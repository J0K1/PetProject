using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetProject.Services;

namespace PetProject.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public readonly UserService _userService;

        public ProfileController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet("/Profile")]
        public async Task<IActionResult> Profile()
        {
            var nick = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(nick))
                return RedirectToAction("Login", "Auth");

            var user = await _userService.GetUserWithDetailsByNickAsync(nick);
            if (user == null)
            {
                //HttpContext.Session.Remove("User");
                //return RedirectToAction("Login", "Auth");
                return NoContent();
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
    }
}
