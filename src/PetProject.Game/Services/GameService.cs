using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetProject.Game.Data;
using PetProject.Shared.Constants;
using PetProject.Shared.Entities;
using PetProject.Shared.Interfaces;
using StackExchange.Redis;

namespace PetProject.Game.Services
{
    public class GameService : IGameService
    {
        private readonly GameDbContext _db;
        private readonly IDatabase _redis;
        private readonly ILogger _logger;

        public GameService(GameDbContext dbContext, IConnectionMultiplexer redis, ILogger<GameService> logger)
        {
            _db = dbContext;
            _redis = redis.GetDatabase();
            _logger = logger;
        }

        public async Task InitializeGamesAsync()
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Models", "json", "games.json");
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning("games.json not found at {Path}", filePath);
                    return;
                }

                var json = await File.ReadAllTextAsync(filePath);
                if (string.IsNullOrWhiteSpace(json)) return;

                var games = JsonConvert.DeserializeObject<List<GameEntity>>(json);
                if (games is null || !games.Any()) return;

                _db.Games.RemoveRange(_db.Games);
                await _db.Games.AddRangeAsync(games);
                await _db.SaveChangesAsync();

                await InvalidateCacheByPrefixAsync("games");
                await _redis.KeyDeleteAsync(CacheKeys.AllGenres);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing games from JSON");
            }
        }

        public async Task<List<GameEntity>> GetAllAsync(int? year = null, string? genre = null)
        {
            string key = CacheKeys.AllGames(year, genre);

            return await GetOrSetAsync(key, async () =>
            {
                var query = _db.Games.AsQueryable();

                if (year.HasValue)
                    query = query.Where(g => g.Year == year.Value);

                if (!string.IsNullOrWhiteSpace(genre))
                {
                    var lower = genre.Trim().ToLower();
                    query = query.Where(g => g.Genre.ToLower().Contains(genre));
                }
                return await query.OrderBy(g => g.Title).ToListAsync();
            }, CacheTimes.Short);
        }

        public async Task<GameEntity?> GetByIdAsync(int id)
        {
            return await GetOrSetAsync(CacheKeys.GameId(id), () => _db.Games.FindAsync(id).AsTask(), CacheTimes.Medium);
        }

        public async Task<List<GameEntity>> GetByTitleAsync(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return await _db.Games.OrderBy(g => g.Title).ToListAsync();

            var lower = title!.Trim().ToLower();
            return await _db.Games
                .Where(g => g.Title.ToLower().Contains(lower))
                .OrderBy(g => g.Title)
                .ToListAsync();
        }

        public async Task<List<GameEntity>> GetByYearAsync(int year)
        {
            return await _db.Games
                .Where(g => g.Year == year)
                .OrderBy(g => g.Title)
                .ToListAsync();
        }

        public async Task AddAsync(GameEntity game)
        {
            await _db.Games.AddAsync(game);
            await _db.SaveChangesAsync();
            await InvalidateCacheByPrefixAsync("games");
        }
        public async Task<bool> UpdateAsync(int id, GameEntity updatedGame)
        {
            var game = await _db.Games.FindAsync(id);
            if (game is null)
                return false;

            game.Title = updatedGame.Title;
            game.Genre = updatedGame.Genre;
            game.Year = updatedGame.Year;
            game.Price = updatedGame.Price;
            game.ImageUrl = updatedGame.ImageUrl;

            await _db.SaveChangesAsync();
            await InvalidateCacheByPrefixAsync("games");
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.Games.FindAsync(id);
            if (existing is null)
                return false;

            _db.Games.Remove(existing);
            await _db.SaveChangesAsync();
            await InvalidateCacheByPrefixAsync("games");
            return true;
        }

        public async Task ClearAllAsync()
        {
            _db.Games.RemoveRange(_db.Games);
            await _db.SaveChangesAsync();
            await InvalidateCacheByPrefixAsync("games");
            await _redis.KeyDeleteAsync(CacheKeys.AllGenres);
        }

        public async Task<List<string>> GetAllGenresAsync()
        {
            return await GetOrSetAsync(CacheKeys.AllGenres, () =>
                _db.Games.Select(g => g.Genre).Distinct().ToListAsync(), CacheTimes.Long);
        }

        private async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiry)
        {
            try
            {
                var cached = await _redis.StringGetAsync(key);
                if (cached.HasValue)
                    return JsonConvert.DeserializeObject<T>(cached!)!;

                var result = await factory();
                var serialized = JsonConvert.SerializeObject(result);
                await _redis.StringSetAsync(key, serialized, expiry);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cache failure for key {key}, falling back to database", key);
                return await factory();
            }
        }

        private async Task InvalidateCacheByPrefixAsync(string prefix)
        {
            try
            {
                var endpoints = _redis.Multiplexer.GetEndPoints();
                var server = _redis.Multiplexer.GetServer(endpoints.First());
                foreach (var key in server.Keys(pattern: $"{prefix}*"))
                {
                    await _redis.KeyDeleteAsync(key);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Unable to invalidate cache by prefix: {prefix}", prefix);
            }
        }
    }
}