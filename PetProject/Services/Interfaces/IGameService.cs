using PetProject.Models;

namespace PetProject.Services.Interfaces
{
    public interface IGameService
    {
        Task InitializeGamesAsync();
        Task<List<GameEntity>> GetAllAsync(int? year = null, string? genre = null);
        Task<GameEntity?> GetByIdAsync(int id);
        Task<List<GameEntity>> GetByTitleAsync(string? title);
        Task<List<GameEntity>> GetByYearAsync(int year);
        Task AddAsync(GameEntity game);
        Task<bool> UpdateAsync(int id, GameEntity game);
        Task<bool> DeleteAsync(int id);
        Task ClearAllAsync();
        Task<List<string>> GetAllGenresAsync();
    }
}
