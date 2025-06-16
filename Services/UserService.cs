using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PetProject.Models;

namespace PetProject.Services
{
    public class UserService
    {
        private List<UserEntity> _users = new List<UserEntity>()
        {
            { new UserEntity {Id = Guid.NewGuid(), Login = "admin", Password = "admin", Email = "", Nick = "admin"} },
            { new UserEntity {Id = Guid.NewGuid(), Login = "guest", Password = "guest", Email = "guest", Nick = "guest"} }
        };

        private AppDBContext _dbContext;
        public UserService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUsersAsync()
        {
            await _dbContext.Users.AddRangeAsync(_users);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<UserEntity>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<List<UserEntity>> GetUsersByNickAsync(string nick)
        {
            return await _dbContext.Users
                .Where(u => u.Nick.ToLower().Contains(nick.ToLower()))
                .ToListAsync();
        }

        public async Task<List<GameEntity>> GetUserGamesByNickAsync(string nick)
        {
            var user = await _dbContext.Users
                .Where(u => u.Nick == nick)
                .Include(u => u.Games)
                .FirstOrDefaultAsync();

            return user?.Games;
        }

        public async Task<List<UserEntity>> GetUserFriendsByNickAsync(string nick)
        {
            var user = await _dbContext.Users
                .Where(u => u.Nick == nick)
                .Include(u => u.Friends)
                .FirstOrDefaultAsync();

            return user?.Friends;
        }

        public async Task AddNewUserAsync(UserEntity user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdatePasswordAsync(UserEntity user, string newPassword)
        {
            var dbUser = await _dbContext.Users.FirstAsync(u => u.Login == user.Login && u.Password == user.Password);
            if (dbUser == null)
                return false;

            dbUser.Password = newPassword;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateNickAsync(UserEntity user, string newNick)
        {
            var dbUser = await _dbContext.Users.FirstAsync(u => u.Login == user.Login && u.Password == user.Password);
            if (dbUser == null) 
                return false;

            dbUser.Nick = newNick;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(UserEntity user)
        {
            var dbUser = await _dbContext.Users.FirstAsync(u => u.Login == user.Login && u.Password == user.Password);
            if (dbUser == null)
                return false;

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BanUserByNickAsync(string nick)
        {
            var dbUser = await _dbContext.Users.FirstAsync(u => u.Nick == nick);
            if(dbUser == null) 
                return false;

            dbUser.IsBanned = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddFriendToUserAsync(string userNick, string friendNick)
        {
            var user = await _dbContext.Users.FirstAsync(u => u.Nick == userNick);
            if (user == null)
                return false;

            var friend = await _dbContext.Users.FirstAsync(u => u.Nick == friendNick);
            if (friend == null)
                return false;

            user.Friends.Add(friend);
            friend.Friends.Add(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddGameToUserAsync(string userNick, string gameTitle)
        {
            var user = await _dbContext.Users.FirstAsync(u => u.Nick == userNick);
            if (user == null)
                return false;

            var game = await _dbContext.Games.FirstAsync(g => g.Title == gameTitle);
            if (game == null)
                return false;

            user.Games.Add(game);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
