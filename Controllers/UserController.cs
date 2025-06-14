using Microsoft.AspNetCore.Mvc;
using PetProject.Models;
using PetProject.Services;

namespace PetProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetUserByLogin{login}")]
        public ActionResult<UserEntity?> GetUserByLogin(string login)
        {
            if (login  == null)
                return NoContent();

            UserEntity? user = _userService.GetUserByLogin(login);
            
            if (user == null)
                return NotFound();
            
            return Ok(user);
        }
    }
}
