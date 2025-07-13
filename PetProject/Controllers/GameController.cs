using Microsoft.AspNetCore.Mvc;
using PetProject.Models;
using PetProject.Services.Interfaces;

namespace PetProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("InitializeGames")]
        public async Task<IActionResult> InitializeGames()
        {
            await _gameService.InitializeGamesAsync();
            return Ok();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] int? year = null, [FromQuery] string? genre = null)
        {
            var games = await _gameService.GetAllAsync(year, genre);
            if (games == null || !games.Any())
                return NoContent();

            return Ok(games);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest();

            var game = await _gameService.GetByIdAsync(id);
            if (game == null) return NotFound();

            return Ok(game);
        }

        [HttpGet("GetByTitle/{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return BadRequest();

            var games = await _gameService.GetByTitleAsync(title);
            if (!games.Any()) return NotFound();

            return Ok(games);
        }

        [HttpGet("GetByYear/{year:int}")]
        public async Task<IActionResult> GetByYear(int year)
        {
            if (year < 1900) return BadRequest();

            var games = await _gameService.GetByYearAsync(year);
            if (!games.Any()) return NotFound();

            return Ok(games);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] GameEntity newGame)
        {
            await _gameService.AddAsync(newGame);
            return CreatedAtAction(nameof(GetById), new { id = newGame.Id }, newGame);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GameEntity updatedGame)
        {
            var ok = await _gameService.UpdateAsync(id, updatedGame);
            if (!ok) return NotFound();

            return NoContent();
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _gameService.DeleteAsync(id);
            if (!ok) return NotFound();

            return NoContent();
        }

        [HttpPost("ClearAllAsync")]
        public async Task<IActionResult> ClearAllAsync()
        {
            await _gameService.ClearAllAsync();
            return Ok();
        }
    }
}
