using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetProject.Models.Views;
using PetProject.Services.Interfaces;
using System.Security.Claims;

namespace PetProject.Controllers
{
    [Authorize]
    public class LibraryController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGameService _gameService;

        public LibraryController(IUserService userService, IGameService gameService)
        {
            _userService = userService;
            _gameService = gameService;
        }

        public async Task<IActionResult> Index(int? selectedId)
        {
            var nick = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(nick))
                return RedirectToAction("Login", "Auth");

            var games = await _userService.GetUserGamesByNickAsync(nick);

            var selected = selectedId.HasValue
                ? games.FirstOrDefault(g => g.Id == selectedId.Value)
                : games.FirstOrDefault();

            var vm = new LibraryViewModel
            {
                Games = games,
                SelectedGame = selected
            };
            return View(vm);
        }

        [HttpPost]
    public async Task<IActionResult> AddGame(int gameId)
    {
        var nick = User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(nick))
            return RedirectToAction("Login", "Auth");

        var game = await _gameService.GetByIdAsync(gameId);
        if (game == null)
            return NotFound();

        await _userService.AddGameToUserAsync(nick, game.Title);

        return RedirectToAction(nameof(Index));
    }
    }
}
