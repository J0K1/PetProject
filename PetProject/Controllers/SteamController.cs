using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetProject.Models.Steam;
using PetProject.Services;
using System.Text.Json;

namespace PetProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SteamController : Controller
    {
        private readonly SteamService _steamService;
        private readonly string _myId = "76561198800777630";

        public SteamController(SteamService steamService)
        {
            _steamService = steamService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return await GetProfile(_myId);
        }

        [Authorize(Policy = "NotBanned")]
        [HttpGet("GetProfile/{id}")]
        public async Task<IActionResult> GetProfile(string id)
        {
            string json = await _steamService.GetPlayerSummary(id);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<SteamPlayerSummaryResponse>(json, options);

            if (data?.Response?.Players == null || !data.Response.Players.Any())
                return NotFound();

            var player = data.Response.Players.FirstOrDefault();

            return View(player);
        }

        [HttpGet("GetAchievements/{steamId}/{appId}")]
        public async Task<IActionResult> GetAchievements(string steamId, string appId)
        {
            var json = await _steamService.GetPlayerAchievements(steamId, appId);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<SteamPlayerAchievementsResponse>(json, options);

            if (data?.PlayerStats == null)
                return NotFound();

            return Ok(data);
        }
    }
}
