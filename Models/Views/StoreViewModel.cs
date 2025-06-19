using System.ComponentModel.DataAnnotations;

namespace PetProject.Models.Views
{
    public class StoreViewModel
    {
        public List<GameEntity> Games { get; set; }
        public List<int> PurchasedIds { get; set; } = new();
        public List<string> AllGenres { get; set; } = new();

        public string? Search { get; set; }
        public string? Genre { get; set; }
        public int? Year { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public bool? Purchased { get; set; }
        public string? SortBy { get; set; }    // "title" или "id"
    }

}
