using Microsoft.EntityFrameworkCore;
using PetProject.Models;

namespace PetProject.Services
{
    public class GameService
    {
        private List<GameEntity> gamesList = new()
        {
            new GameEntity { Id = 1, Title = "The Witcher 3", Genre = "RPG", Year = 2015 },
            new GameEntity { Id = 2, Title = "Cyberpunk 2077", Genre = "RPG", Year = 2020 },
            new GameEntity { Id = 3, Title = "Red Dead Redemption 2", Genre = "Action-Adventure", Year = 2018 },
            new GameEntity { Id = 4, Title = "Elden Ring", Genre = "Action RPG", Year = 2022 },
            new GameEntity { Id = 5, Title = "God of War", Genre = "Action", Year = 2018 },
            new GameEntity { Id = 6, Title = "Minecraft", Genre = "Sandbox", Year = 2011 },
            new GameEntity { Id = 7, Title = "Hollow Knight", Genre = "Metroidvania", Year = 2017 },
            new GameEntity { Id = 8, Title = "Stardew Valley", Genre = "Simulation", Year = 2016 },
            new GameEntity { Id = 9, Title = "Portal 2", Genre = "Puzzle", Year = 2011 },
            new GameEntity { Id = 10, Title = "DOOM Eternal", Genre = "Shooter", Year = 2020 }
        };

        private readonly AppDBContext _dbContext;

        public GameService(AppDBContext context)
        {
            _dbContext = context;
        }

        public async Task AddGamesAsync()
        {
            await _dbContext.Games.AddRangeAsync(gamesList);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<GameEntity>> GetAllAsync()
        {
            return await _dbContext.Games
                .OrderBy(g => g.Id)
                .ToListAsync();
        }

        public async Task<GameEntity?> GetByIdAsync(int id)
        {
            return await _dbContext.Games.FindAsync(id);
        }

        public async Task<List<GameEntity>> GetByTitleAsync(string title)
        {
            return await _dbContext.Games
                .Where(g => g.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync();
        }

        public async Task<List<GameEntity>> GetByYearAsync(int year)
        {
            return await _dbContext.Games
                .Where(g => g.Year == year)
                .ToListAsync();
        }

        public async Task AddAsync(GameEntity game)
        {
            await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int id, GameEntity updated)
        {
            var game = await _dbContext.Games.FindAsync(id);
            if (game == null)
                return false;
            game.Title = updated.Title;
            game.Genre = updated.Genre;
            game.Year = updated.Year;

            await _dbContext.SaveChangesAsync();
            return true;        
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var game = await _dbContext.Games.FindAsync(id);
            if (game == null) return false;

            _dbContext.Games.Remove(game);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
