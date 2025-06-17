using Microsoft.AspNetCore.Mvc;
using PetProject.Models;
using PetProject.Services;
using System.Threading.Tasks;
using PetProject.Models.Views;

namespace PetProject.Controllers
{
    public class PagesController : Controller
    {
        private readonly GameService _gameService;
        private readonly UserService _userService;

        public PagesController(GameService gameService, UserService userService)
        {
            _gameService = gameService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Store));
        }

        public IActionResult Store()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid)
                return View(loginViewModel);

            var user = await _userService.GetUserAsync(loginViewModel.Login, loginViewModel.Password);
            if(user == null)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(loginViewModel);
            }

            HttpContext.Session.SetString("User", user.Nick);
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(!ModelState.IsValid)
                return View(registerViewModel);

            var existing = await _userService.GetAllUsersAsync();
            if(existing.Any(u => u.Login == registerViewModel.Login))
            {
                ModelState.AddModelError("Login", "Пользователь уже существует");
                return View(registerViewModel);
            }

            await _userService.AddNewUserAsync(new UserEntity
            {
                Login = registerViewModel.Login,
                Password = registerViewModel.Password,
                Nick = registerViewModel.Nick
            });

            HttpContext.Session.SetString("User", registerViewModel.Nick);
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var nick = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(nick))
                return RedirectToAction(nameof(Login));

            var user = await _userService.GetUserWithDetailsByNickAsync(nick);
            if(user == null)
            {
                HttpContext.Session.Remove("User");
                return RedirectToAction(nameof(Login));
            }

            return View(user);
        }

        [HttpGet("/Profile/{nick}")]
        public async Task<IActionResult> Profile(string nick)
        {
            var user = await _userService.GetUserWithDetailsByNickAsync(nick);
            if (user == null)
                return NotFound();

            return View(user);
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
