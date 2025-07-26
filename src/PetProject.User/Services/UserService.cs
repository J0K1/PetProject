using Microsoft.EntityFrameworkCore;
using PetProject.Shared.Entities;
using PetProject.Shared.Interfaces;
using PetProject.User.Data;
using PetProject.Shared.Enums;

namespace PetProject.User.Services
{
    public class UserService : IUserService
    {
        private static readonly List<UserEntity> _users = new List<UserEntity>()
        {
            { new UserEntity {Id = Guid.NewGuid(), Login = "admin", Password = "admin", Email = "", Nick = "admin", Role = UserRole.Admin } },
            { new UserEntity {Id = Guid.NewGuid(), Login = "guest", Password = "guest", Email = "guest", Nick = "guest"} }
        };

        private readonly UserDbContext _db;
        private readonly ILogger<UserService> _logger;
        public UserService(UserDbContext dbContext, ILogger<UserService> logger)
        {
            _db = dbContext;
            _logger = logger;
        }

        public async Task InitializeUsersAsync()
        {
            await _db.Users.AddRangeAsync(_users);
            await _db.SaveChangesAsync();
        }

        public async Task<List<UserEntity>> GetAllUsersAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<UserEntity?> GetUserAsync(string login, string password)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
        }

        public async Task<UserEntity?> GetUserWithDetailsByNickAsync(string nick)
        {
            return await _db.Users
                .Include(u => u.Friends)
                .Include(u => u.Games)
                .FirstOrDefaultAsync(u => u.Nick == nick);
        }

        public async Task<List<UserEntity>> GetUsersByNickAsync(string? nick)
        {
            var query = _db.Users.AsQueryable();

            if (!string.IsNullOrEmpty(nick))
            {
                var lowed = nick.ToLower();
                query = query.Where(u => u.Nick.ToLower().Contains(lowed));
            }
            return await query.ToListAsync();
        }

        public async Task<List<GameEntity>> GetUserGamesByNickAsync(string nick)
        {
            var user = await _db.Users
                .Include(u => u.Games)
                .FirstOrDefaultAsync(u => u.Nick == nick);

            return user?.Games.ToList() ?? new();
        }

        public async Task<List<UserEntity>> GetUserFriendsByNickAsync(string nick)
        {
            var user = await _db.Users
                .Include(u => u.Friends)
                .FirstOrDefaultAsync(u => u.Nick == nick);

            return user?.Friends.ToList() ?? new();
        }

        public async Task<bool> AddNewUserAsync(UserEntity user)
        {
            if (await _db.Users.AnyAsync(u => u.Login == user.Login))
                return false;

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserAsync(Guid id, UserEntity updatedUser)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return false;

            user.Nick = updatedUser.Nick;
            user.Role = updatedUser.Role;
            user.IsBanned = updatedUser.IsBanned;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePasswordAsync(string login, string password, string newPassword)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
            if (user == null)
                return false;

            user.Password = newPassword;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateNickAsync(string login, string password, string newNick)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
            if (user == null)
                return false;

            user.Nick = newNick;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(string login, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
            if (user == null)
                return false;

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BanUserByNickAsync(string nick)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Nick == nick);
            if (user == null)
                return false;

            user.IsBanned = true;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddFriendToUserAsync(string userNick, string friendNick)
        {
            var user = await _db.Users
                .Include(u => u.Friends)
                .FirstOrDefaultAsync(u => u.Nick == userNick);

            var friend = await _db.Users
                .Include(u => u.Friends)
                .FirstOrDefaultAsync(u => u.Nick == friendNick);

            if (user is null || friend is null || user.Id == friend.Id)
                return false;

            if (!user.Friends.Any(f => f.Id == friend.Id))
                user.Friends.Add(friend);

            if (!friend.Friends.Any(f => f.Id == user.Id))
                friend.Friends.Add(user);

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddGameToUserAsync(string userNick, string gameTitle)
        {
            //var user = await _db.Users.Include(u => u.Games).FirstOrDefaultAsync(u => u.Nick == userNick);
            //var game = await _db.Games.FirstOrDefaultAsync(g => g.Title == gameTitle);

            //if (user is null || game is null)
            //    return false;

            //if (!user.Games.Any(g => g.Id == game.Id))
            //    user.Games.Add(game);

            //await _db.SaveChangesAsync();
            return true;
        }
    }
}