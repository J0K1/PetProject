using Microsoft.AspNetCore.Mvc;

namespace PetProject.Controllers
{
    public class PagesController : Controller
    {
        public async Task<IActionResult> Index() => RedirectToAction("Store");
        
        public async Task<IActionResult> Store() => View();
        
        public async Task<IActionResult> GameDetails(int id) => View(model: id);
        
        [HttpGet]
        public async Task<IActionResult> Register() => View();
        
        [HttpGet]
        public async Task<IActionResult> Login() => View();

        public async Task<IActionResult> Profile(string nick) => View(model: nick);


    }
}
