using PetProject.Models;

namespace PetProject.Services
{
    public class UserService
    {
        private Dictionary<string, User> _users = new Dictionary<string, User>()
        {
            { "admin", new User {Id = Guid.NewGuid(), Login = "admin", Password = "admin", Email = "", NickName = "admin"} },
            { "guest", new User {Id = Guid.NewGuid(), Login = "guest", Password = "guest", Email = "guest", NickName = "guest"} }
        };

        public User? GetUserByLogin(string login)
        {
            return _users[login];
        }
    }
}
