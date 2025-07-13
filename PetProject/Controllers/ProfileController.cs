using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetProject.Models.Views;
using PetProject.Services.Interfaces;

namespace PetProject.Controllers
{
    [Authorize]
    [Route("Profile")]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        public ProfileController(IUserService userService)
            => _userService = userService;

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var currentNick = User.Identity?.Name;
            if (string.IsNullOrEmpty(currentNick))
                return RedirectToAction("Login", "Auth");

            var me = await _userService.GetUserWithDetailsByNickAsync(currentNick);
            if (me == null)
                return NoContent();

            var vm = new ProfileViewModel
            {
                User = me,
                IsOwnProfile = true,
                IsFriend = false,
                CurrentUserNick = currentNick
            };
            return View("Index", vm);
        }

        [HttpGet("{nick}")]
        public async Task<IActionResult> Index(string nick)
        {
            var currentNick = User.Identity?.Name;

            var user = await _userService.GetUserWithDetailsByNickAsync(nick);
            if (user == null)
                return NotFound();

            bool isOwn = !string.IsNullOrEmpty(currentNick) && currentNick == nick;
            bool isFriend = false;
            if (!isOwn && !string.IsNullOrEmpty(currentNick))
            {
                var me = await _userService.GetUserWithDetailsByNickAsync(currentNick);
                isFriend = me?.Friends.Any(f => f.Nick == nick) == true;
            }

            var vm = new ProfileViewModel
            {
                User = user,
                IsOwnProfile = isOwn,
                IsFriend = isFriend,
                CurrentUserNick = currentNick
            };
            return View("Index", vm);
        }
    }
}
