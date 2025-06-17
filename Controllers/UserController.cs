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

        [HttpPost("AddUsers")]
        public async Task<IActionResult> AddUsers()
        {
            await _userService.AddUsersAsync();
            return Ok();
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<UserEntity>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("GetUser/{login}/{password}")]
        public async Task<ActionResult<UserEntity>> GetUser(string login, string password)
        {
            var user = await _userService.GetUserAsync(login, password);
            if(user == null) 
                return NoContent();

            return Ok(user);
        }

        [HttpGet("GetUserByNick/{nick}")]
        public async Task<ActionResult<List<UserEntity>>> GetUserByNick(string nick)
        {
            var users = await _userService.GetUsersByNickAsync(nick);
            return Ok(users);
        }

        [HttpGet("GetUserGamesByNick/{nick}")]
        public async Task<ActionResult<List<GameEntity>>> GetUserGamesByNick(string nick)
        {
            var games = await _userService.GetUserGamesByNickAsync(nick);
            return Ok(games);
        }

        [HttpGet("GetUserFriendsByNick/{nick}")]
        public async Task<ActionResult<List<UserEntity>>> GetUserFriendsByNick(string nick)
        {
            var friend = await _userService.GetUserFriendsByNickAsync(nick);
            return Ok(friend);
        }

        [HttpPost("AddNewUser")]
        public async Task<ActionResult> AddNewUser(string login, string password, string nick)
        {
            var newUser = new UserEntity { Login = login, Password = password, Nick = nick };
            await _userService.AddNewUserAsync(newUser);
            return Ok(); 
        }

        [HttpPut("UpdatePassword/{login}/{password}")]
        public async Task<ActionResult> UpdatePassword(string login, string password, string newPassword)
        {
            bool isConfirm = await _userService.UpdatePasswordAsync(login, password, newPassword);
            if (isConfirm)
                return Ok();
            else
                return BadRequest(login);
        }

        [HttpPut("UpdateNick/{login}/{password}")]
        public async Task<ActionResult> UpdateNick(string login, string password, string newNick)
        {
            bool isConfirm = await _userService.UpdateNickAsync(login, password, newNick);
            if (isConfirm)
                return Ok();
            else
                return BadRequest(login);
        }

        [HttpDelete("DeleteUser/{login}/{password}")]
        public async Task<ActionResult> DeleteUser(string login, string password)
        {
            bool isConfirm = await _userService.DeleteUserAsync(login, password);
            if (isConfirm)
                return Ok();
            else
                return BadRequest(login);
        }

        [HttpPut("BanUserByNick/{nick}")]
        public async Task<ActionResult> BanUserByNick(string nick)
        {
            bool isConfirm = await _userService.BanUserByNickAsync(nick);
            if (isConfirm)
                return Ok();
            else
                return BadRequest(nick);
        }

        [HttpPut("AddFriendToUser")]
        public async Task<ActionResult> AddFriendToUser(string userNick, string friendNick)
        {
            bool isConfirm = await _userService.AddFriendToUserAsync(userNick, friendNick);
            if (isConfirm)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPut("AddGameToUser")]
        public async Task<ActionResult> AddGameToUser(string nick, string gameTitle)
        {
            bool isConfirm = await _userService.AddGameToUserAsync(nick, gameTitle);
            if (isConfirm)
                return Ok();
            else
                return BadRequest();
        }
    }
}
