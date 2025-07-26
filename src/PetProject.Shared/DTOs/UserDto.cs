using PetProject.Shared.Enums;

namespace PetProject.Shared.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Nick { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;
        public bool IsActive { get; set; } = true;
        public bool IsBanned { get; set; } = false;
        public string PhotoUrl { get; set; } = string.Empty;
    }
}
