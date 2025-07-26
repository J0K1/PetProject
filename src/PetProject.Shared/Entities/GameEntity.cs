namespace PetProject.Shared.Entities
{
    public class GameEntity
    {
        public int Id { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int Year { get; set; } = 1901;
        public decimal Price { get; set; } = 30;

        public string ImageUrl { get; set; } = "https://imgpng.ru/download/87340";
    }
}
