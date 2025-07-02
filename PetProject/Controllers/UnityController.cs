using Microsoft.AspNetCore.Mvc;

namespace PetProject.Controllers
{
    public class UnityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
