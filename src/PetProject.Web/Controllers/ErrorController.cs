using Microsoft.AspNetCore.Mvc;

namespace PetProject.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult Error404()
        {
            Response.StatusCode = 404;
            return View("Error404");
        }
    }
}
