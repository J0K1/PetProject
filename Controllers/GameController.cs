using Microsoft.AspNetCore.Mvc;
using PetProject.Models;
using PetProject.Services;

namespace PetProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : Controller
    {
        private readonly GameService _gameService;
        private readonly ILogger<GamesController> _logger;

        public GamesController(GameService gameService, ILogger<GamesController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpPost("AddGames")]
        public async Task<IActionResult> AddGames()
        {
            await _gameService.AddGamesAsync();
            return Ok();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] int? year = null, [FromQuery] string genre = null)
        {
            var games = await _gameService.GetAllAsync(year, genre);
            if (games == null || !games.Any())
                return NoContent();

            _logger.LogInformation("Fetched all games");
            return Ok(games);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest();

            var game = await _gameService.GetByIdAsync(id);
            if (game == null) return NotFound();

            _logger.LogInformation("Fetched game {Id}", id);
            return Ok(game);
        }

        [HttpGet("GetByTitle/{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return BadRequest();

            var games = await _gameService.GetByTitleAsync(title);
            if (!games.Any()) return NotFound();

            _logger.LogInformation("Searched games by title ‘‘{Title}’’", title);
            return Ok(games);
        }

        [HttpGet("GetByYear/{year:int}")]
        public async Task<IActionResult> GetByYear(int year)
        {
            if (year < 1900) return BadRequest();

            var games = await _gameService.GetByYearAsync(year);
            if (!games.Any()) return NotFound();

            _logger.LogInformation("Fetched games from year {Year}", year);
            return Ok(games);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] GameEntity newGame)
        {
            await _gameService.AddAsync(newGame);
            _logger.LogInformation("Created game {Id}", newGame.Id);
            return CreatedAtAction(nameof(GetById), new { id = newGame.Id }, newGame);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GameEntity updatedGame)
        {
            var ok = await _gameService.UpdateAsync(id, updatedGame);
            if (!ok) return NotFound();

            _logger.LogInformation("Updated game {Id}", id);
            return NoContent();
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _gameService.DeleteAsync(id);
            if (!ok) return NotFound();

            _logger.LogInformation("Deleted game {Id}", id);
            return NoContent();
        }

        [HttpPost("DeleteAllGames")]
        public async Task<IActionResult> DeleteAllGames()
        {
            await _gameService.DeleteAllGamesAsync();
            _logger.LogInformation("Deleted all games");
            return Ok();
        }
    }
}
