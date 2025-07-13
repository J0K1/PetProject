using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Enums;
using PetProject.Models;
using PetProject.Services;
using Polly;

namespace PetProject.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly AppDBContext _dbContext;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _dbContext = new AppDBContext(options);
            var loggerMock = new Mock<ILogger<UserService>>();

            _userService = new UserService(_dbContext, loggerMock.Object);
        }

        [Fact]
        public async Task GetUserAsync_ReturnsCorrectUser()
        {
            var user = new UserEntity { Login = "test", Password = "pass", Nick = "nick" };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var result = await _userService.GetUserAsync("test", "pass");

            Assert.NotNull(result);
            Assert.Equal("nick", result.Nick);
        }

        [Fact]
        public async Task GetUserWithDetailsByNickAsync_ReturnsUserWithFriendsAndGames()
        {
            var friend = new UserEntity { Nick = "friend" };
            var game = new GameEntity { Title = "game" };
            var user = new UserEntity { Nick = "user", Friends = new List<UserEntity> { friend }, Games = new List<GameEntity> { game } };

            _dbContext.Users.AddRange(user, friend);
            _dbContext.Games.Add(game);
            await _dbContext.SaveChangesAsync();

            var result = await _userService.GetUserWithDetailsByNickAsync("user");

            Assert.NotNull(result);
            Assert.Single(result.Friends);
            Assert.Single(result.Games);
        }

        [Fact]
        public async Task AddNewUserAsync_ReturnsFalseIfLoginExists()
        {
            var existing = new UserEntity { Login = "same", Password = "123" };
            var newUser = new UserEntity { Login = "same", Password = "456" };
            _dbContext.Users.Add(existing);
            await _dbContext.SaveChangesAsync();

            var result = await _userService.AddNewUserAsync(newUser);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesUserData()
        {
            var user = new UserEntity { Nick = "old", Role = UserRole.User };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var updated = new UserEntity { Nick = "new", Role = UserRole.Admin };
            var result = await _userService.UpdateUserAsync(user.Id, updated);

            var changed = await _dbContext.Users.FindAsync(user.Id);
            Assert.True(result);
            Assert.Equal("new", changed?.Nick);
            Assert.Equal(UserRole.Admin, changed?.Role);
        }

        [Fact]
        public async Task AddFriendToUserAsync_AddsFriendsBothWays()
        {
            var user = new UserEntity { Nick = "A", Friends = new List<UserEntity>() };
            var friend = new UserEntity { Nick = "B", Friends = new List<UserEntity>() };
            _dbContext.Users.AddRange(user, friend);
            await _dbContext.SaveChangesAsync();

            var result = await _userService.AddFriendToUserAsync("A", "B");

            Assert.True(result);
            var userFromDb = await _dbContext.Users.Include(u => u.Friends).FirstAsync(u => u.Nick == "A");
            var friendFromDb = await _dbContext.Users.Include(u => u.Friends).FirstAsync(u => u.Nick == "B");

            Assert.Contains(friendFromDb, userFromDb.Friends);
            Assert.Contains(userFromDb, friendFromDb.Friends);
        }

        [Fact]
        public async Task AddGameToUserAsync_AddsGameIfNotExists()
        {
            var game = new GameEntity { Title = "Game" };
            var user = new UserEntity { Nick = "Nick", Games = new List<GameEntity>() };

            _dbContext.Users.Add(user);
            _dbContext.Games.Add(game);
            await _dbContext.SaveChangesAsync();

            var result = await _userService.AddGameToUserAsync("Nick", "Game");

            Assert.True(result);
            var fromDb = await _dbContext.Users.Include(u => u.Games).FirstAsync(u => u.Nick == "Nick");
            Assert.Contains(fromDb.Games, g => g.Title == "Game");
        }
        
        [Fact]
        public async Task AddNewUserAsync_ShouldReturnTrue_WhenUserIsUnique2()
        {
            var user = new UserEntity
            {
                Id = Guid.NewGuid(),
                Login = "testuser",
                Password = "123",
                Email = "test@example.com",
                Nick = "test"
            };

            var result = await _userService.UpdateUserAsync(user.Id, user);
            Assert.False(result);
        }
    }
}
