using PetProject.Models;

namespace PetProject.Services
{
    public class UserService
    {
        private Dictionary<string, UserEntity> _users = new Dictionary<string, UserEntity>()
        {
            { "admin", new UserEntity {Id = Guid.NewGuid(), Login = "admin", Password = "admin", Email = "", NickName = "admin"} },
            { "guest", new UserEntity {Id = Guid.NewGuid(), Login = "guest", Password = "guest", Email = "guest", NickName = "guest"} }
        };

        public UserEntity? GetUserByLogin(string login)
        {
            return _users[login];
        }
    }
}
