using Microsoft.AspNetCore.Mvc;
using PetProject.Models.Views;
using PetProject.Services;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PetProject.Controllers
{
    public class StoreController : Controller
    {
        private readonly GameService _gameService;
        private readonly UserService _userService;
        
        public StoreController(GameService gameService, UserService userService)
        {
            _gameService = gameService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Main));
        }

        [HttpGet]
        public async Task<IActionResult> Main(string? search, string? genre, int? year, 
            decimal? priceFrom, decimal? priceTo, bool? purchased, string? sortBy)
        {
            var games = await _gameService.GetAllAsync();
            if (!string.IsNullOrEmpty(search))
                games = games
                    .Where(g => g.Title.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            if (!string.IsNullOrEmpty(genre))
                games = games.Where(g => g.Genre == genre).ToList();

            if (year.HasValue)
                games = games.Where(g => g.Year == year.Value).ToList();

            if (priceFrom.HasValue)
                games = games.Where(g => g.Price >= priceFrom.Value).ToList();

            if (priceTo.HasValue)
                games = games.Where(g => g.Price <= priceTo.Value).ToList();

            games = sortBy switch
            {
                "title" => games.OrderBy(g => g.Title).ToList(),
                "id" => games.OrderBy(g => g.Id).ToList(),
                _ => games
            };

            List<int> purchasedIds = new();
            if (purchased.HasValue && User.Identity.IsAuthenticated)
            {
                var nick = User.FindFirstValue(ClaimTypes.Name)!;
                var userGames = await _userService.GetUserGamesByNickAsync(nick);
                purchasedIds = userGames.Select(g => g.Id).ToList();

                if (purchased.Value)
                    games = games.Where(g => purchasedIds.Contains(g.Id)).ToList();
                else
                    games = games.Where(g => !purchasedIds.Contains(g.Id)).ToList();
            }

            var allGenres = await _gameService.GetAllGenresAsync();

            var vm = new StoreViewModel
            {
                Games = games,
                PurchasedIds = purchasedIds,
                AllGenres = allGenres,
                Search = search,
                Genre = genre,
                Year = year,
                PriceFrom = priceFrom,
                PriceTo = priceTo,
                Purchased = purchased,
                SortBy = sortBy
            };

            return View(vm);
        }

        public async Task<IActionResult> GameDetails(int id)
        {
            if (id <= 0)
                return NotFound();

            var game = await _gameService.GetByIdAsync(id);
            if (game == null)
                return NotFound();

            return View(game);
        }
    }
}
