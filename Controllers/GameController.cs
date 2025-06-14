using Microsoft.AspNetCore.Components.Forms;
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
        public async Task<ActionResult> AddGames()
        {
            await _gameService.AddGamesAsync();
            return Ok();
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GameEntity>>> GetAll()
        {
            var games = await _gameService.GetAllAsync();
            if (games == null || !games.Any())
                return NoContent();

            _logger.LogInformation("Get all");
            return Ok(games);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<GameEntity>> GetById(int id)
        {
            if (id <= 0)
                return NoContent();

            var game = await _gameService.GetByIdAsync(id);

            if (game == null)
                return NotFound();

            _logger.LogInformation("Get by id");
            return Ok(game);
        }

        [HttpGet("GetByTitle/{title}")]
        public async Task<ActionResult<List<GameEntity>>> GetByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                return NoContent();

            var games = await _gameService.GetByTitleAsync(title);

            if (games == null || !games.Any())
                return NotFound();

            _logger.LogInformation("Get by title");
            return Ok(games);
        }

        [HttpGet("GetByYear/{year:int}")]
        public async Task<ActionResult<List<GameEntity>>> GetByYear(int year)
        {
            if (year <= 1900)
                return NoContent();

            var games = await _gameService.GetByYearAsync(year);

            if (games == null || !games.Any())
                return NotFound();

            _logger.LogInformation("Get by year");
            return Ok(games);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<GameEntity>> Create(int id, string title, string genre, int year)
        {
            var game = new GameEntity { Id = id, Title = title, Genre = genre, Year = year };
            await _gameService.AddAsync(game);
            _logger.LogInformation("Create");
            return CreatedAtAction(nameof(GetById), new { id = game.Id }, game);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult> Update(int id, string title, string genre, int year)
        {
            var updatedGame = new GameEntity { Id = id, Title = title, Genre = genre, Year = year};
            bool result = await _gameService.UpdateAsync(id, updatedGame);
            if (!result)
                return NotFound();

            _logger.LogInformation("Update");
            return NoContent();
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool result = await _gameService.DeleteAsync(id);
            if (!result)
                return NotFound();

            _logger.LogInformation("Delete");
            return NoContent();
        }
    }
}
