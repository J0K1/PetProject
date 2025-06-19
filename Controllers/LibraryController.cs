using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyModel;
using PetProject.Models.Views;
using PetProject.Services;
using System.Security.Claims;

namespace PetProject.Controllers
{
    [Authorize]
    public class LibraryController : Controller
    {
        private readonly UserService _userService;
        private readonly GameService _gameService;

        public LibraryController(UserService userService, GameService gameService)
        {
            _userService = userService;
            _gameService = gameService;
        }

        public async Task<IActionResult> Index()
        {
            return RedirectToAction(nameof(Library));
        }

        public async Task<IActionResult> Library(int? selectedId)
        {
            // 1) Получаем ник из Claims
            var nick = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(nick))
                return Challenge(); // или RedirectToAction("Login", "Auth");

            // 2) Подгружаем игры по нику
            var games = await _userService.GetUserGamesByNickAsync(nick);

            // 3) Выбираем текущую
            var selected = selectedId.HasValue
                ? games.FirstOrDefault(g => g.Id == selectedId.Value)
                : games.FirstOrDefault();

            // 4) Формируем ViewModel
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
        // 1) достаём ник из claims
        var nick = User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(nick))
            return Challenge();

        // 2) получаем титул игры по id
        var game = await _gameService.GetByIdAsync(gameId);
        if (game == null)
            return NotFound();

        // 3) добавляем в библиотеку
        await _userService.AddGameToUserAsync(nick, game.Title);

        // 4) редиректим в библиотеку
        return RedirectToAction(nameof(Index));
    }
    }
}
