using PetProject.Enums;

namespace PetProject.Models
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Nick { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;

        public bool IsActive { get; set; } = true;
        public bool IsBanned { get; set; } = false;

        public List<UserEntity> Friends { get; set; } = new();
        public List<GameEntity> Games { get; set; } = new();
    }
}
