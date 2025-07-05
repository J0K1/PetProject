using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetProject.Models;
using PetProject.Models.Views;

namespace PetProject.Services
{
    public class GameService
    {
        private readonly AppDBContext _dbContext;

        public GameService(AppDBContext context)
            => _dbContext = context;

        public async Task AddGamesAsync()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Models", "json", "games.json");
            var json = await File.ReadAllTextAsync(filePath);
            if (string.IsNullOrWhiteSpace(json)) return;

            var games = JsonConvert.DeserializeObject<List<GameEntity>>(json);
            if (games == null || games.Count == 0) return;

            _dbContext.Games.RemoveRange(_dbContext.Games);
            await _dbContext.Games.AddRangeAsync(games);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<GameEntity>> GetAllAsync(int? year = null, string genre = null)
        {
            var query = _dbContext.Games.AsQueryable();

            if(!year.HasValue && string.IsNullOrWhiteSpace(genre))
                return await _dbContext.Games.ToListAsync();

            if (year.HasValue)
                query = query.Where(g => g.Year == year.Value);

            if (!string.IsNullOrWhiteSpace(genre))
            {
                var lower = genre.Trim().ToLower();
                query = query.Where(g => g.Genre.ToLower().Contains(lower));
            }

            return await query
                .OrderBy(g => g.Title)
                .ToListAsync();
        }

        public Task<GameEntity?> GetByIdAsync(int id)
            => _dbContext.Games.FindAsync(id).AsTask();

        public async Task<List<GameEntity>> GetByTitleAsync(string? title)
        {
            if (string.IsNullOrEmpty(title))
                return await _dbContext.Games.ToListAsync();

            var lower = title.Trim().ToLower();
            return await _dbContext.Games
                .Where(g => g.Title.ToLower().Contains(lower))
                .OrderBy(g => g.Title)
                .ToListAsync();
        }

        public async Task<List<GameEntity>> GetByYearAsync(int year)
            => await _dbContext.Games
                .Where(g => g.Year == year)
                .OrderBy(g => g.Title)
                .ToListAsync();

        public async Task AddAsync(GameEntity game)
        {
            await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> UpdateAsync(int id, GameEntity updatedGame)
        {
            var game = await _dbContext.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (game == null) return false;

            game.Title = updatedGame.Title;
            game.Genre = updatedGame.Genre;
            game.Year = updatedGame.Year;
            game.Price = updatedGame.Price;
            game.ImageUrl = updatedGame.ImageUrl;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _dbContext.Games.FindAsync(id);
            if (existing == null) return false;

            _dbContext.Games.Remove(existing);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAllGamesAsync()
        {
            _dbContext.Games.RemoveRange(_dbContext.Games);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<string>> GetAllGenresAsync()
        {
            return await _dbContext.Games.Select(g => g.Genre).Distinct().ToListAsync();
        }
    }
}
