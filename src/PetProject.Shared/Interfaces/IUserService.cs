using PetProject.Shared.Entities;

namespace PetProject.Shared.Interfaces
{
    public interface IUserService
    {
        Task InitializeUsersAsync();

        Task<List<UserEntity>> GetAllUsersAsync();
        Task<UserEntity?> GetUserAsync(string login, string password);
        Task<UserEntity?> GetUserWithDetailsByNickAsync(string nick);
        Task<List<UserEntity>> GetUsersByNickAsync(string? nick);

        Task<List<GameEntity>> GetUserGamesByNickAsync(string nick);
        Task<List<UserEntity>> GetUserFriendsByNickAsync(string nick);

        Task<bool> AddNewUserAsync(UserEntity user);
        Task<bool> UpdateUserAsync(Guid id, UserEntity updatedUser);
        Task<bool> UpdatePasswordAsync(string login, string password, string newPassword);
        Task<bool> UpdateNickAsync(string login, string password, string newNick);
        Task<bool> DeleteUserAsync(string login, string password);
        Task<bool> BanUserByNickAsync(string nick);

        Task<bool> AddFriendToUserAsync(string userNick, string friendNick);
        Task<bool> AddGameToUserAsync(string userNick, string gameTitle);
    }
}
