namespace PetProject.Models
{
    public class GameEntity
    {
        public int Id { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int Year { get; set; } = 1901;
        public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
    }
}
