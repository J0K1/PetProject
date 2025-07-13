using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetProject.Enums;
using PetProject.Models;
using PetProject.Models.Views;
using PetProject.Services.Interfaces;
using System.Globalization;

namespace PetProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IUserService _userService;
        public AdminController(IGameService gameService, IUserService userService)
        {
            _gameService = gameService;
            _userService = userService;
        }

        [HttpGet("Admin")]
        public async Task<IActionResult> Index(string? userSearch, string? gameSerach)
        {
            var users = await _userService.GetUsersByNickAsync(userSearch);
            users = users.OrderBy(u => u.Nick).ToList();

            var games = await _gameService.GetByTitleAsync(gameSerach);
            games = games.OrderBy(g => g.Id).ToList();


            return View(new AdminViewModel
            {
                Users = users,
                Games = games
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserAsync(Guid id, string nick, UserRole role, bool isBanned)
        {
            var updatedUser = new UserEntity
            {
                Id = id,
                Nick = nick,
                Role = role,
                IsBanned = isBanned
            };

            await _userService.UpdateUserAsync(id, updatedUser);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGameAsync(int id, string title, string genre, int year, string price, string imageURL)
        {
            if (!decimal.TryParse(price, NumberStyles.Any,
                    CultureInfo.InvariantCulture, out var parsedPrice))
            {
                ModelState.AddModelError("price", "Неверный формат цены");
                return RedirectToAction(nameof(Index));
            }

            var updatedGame = new GameEntity
            {
                Id = id,
                Title = title,
                Genre = genre,
                Year = year,
                Price = parsedPrice,
                ImageUrl = imageURL
            };

            await _gameService.UpdateAsync(id, updatedGame);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CreateGameAsync(GameEntity game)
        {
            var games = await _gameService.GetAllAsync();
            game.Id = games.Count + 1;

            await _gameService.AddAsync(game);    
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGameAsync(int id)
        {
            await _gameService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
