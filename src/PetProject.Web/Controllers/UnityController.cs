using Microsoft.AspNetCore.Mvc;

namespace PetProject.Web.Controllers
{
    public class UnityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
