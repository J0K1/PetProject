using PetProject.Shared.Enums;

namespace PetProject.Shared.Entities
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

        public string PhotoUrl { get; set; } = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fstatic.vecteezy.com%2Fsystem%2Fresources%2Fthumbnails%2F036%2F280%2F651%2Fsmall_2x%2Fdefault-avatar-profile-icon-social-media-user-image-gray-avatar-icon-blank-profile-silhouette-illustration-vector.jpg&f=1&nofb=1&ipt=f399372f42b355ef9bccdcd95ddaa04492a2d754be88eb00d250cf215aac73e9";
    }
}
