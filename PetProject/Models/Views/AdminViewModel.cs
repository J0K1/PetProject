namespace PetProject.Models.Views
{
    public class AdminViewModel
    {
        public List<UserEntity> Users { get; set; } = new List<UserEntity>();

        public List<GameEntity> Games { get; set; } = new List<GameEntity>();
    }
}
