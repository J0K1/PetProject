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
        private ILogger _logger;
        public GamesController(GameService gameService, ILogger<GamesController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<Game>> GetAll()
        {
            if (DBSize < 0)
                return NoContent();

            _logger.LogInformation("Get all");
            return Ok(_gameService.GetAll());
        }

        [HttpGet("GetById/{id:int}")]
        public ActionResult<Game> GetById(int id)
        {
            if(id <= 0 || id > DBSize)
                return NoContent();

            var game = _gameService.GetById(id);

            if (game == null)
                return NotFound();

            _logger.LogInformation("Get by id");
            return Ok(game);
        }

        [HttpGet("GetByTitle/{title}")]
        public ActionResult<Game> GetByTitle(string title)
        {
            if(title == null || title.Length < 1)
                return NoContent();

            var game = _gameService.GetByTitle(title);

            if(game == null)
                return NotFound();

            _logger.LogInformation("Get by title");
            return Ok(game);
        }

        [HttpGet("GetByYear/{year:int}")]
        public ActionResult<List<Game>> GetByYear(int year)
        {
            if(year <= 1900)
                return NoContent();

            List<Game> games = _gameService.GetByYear(year);

            if(games == null)
                return NotFound();

            _logger.LogInformation("Get by year");
            return Ok(games);
        }

        [HttpPost("Create")]
        public ActionResult<Game> Create(Game game)
        {
            if(game == null) 
                return NoContent();

            _gameService.Add(game);
            _logger.LogInformation("Create");
            return CreatedAtAction(nameof(GetById), new { id = game.Id }, game);
        }

        [HttpPut("Update")]
        public ActionResult<Game> Update(int id, Game updatedGame)
        {
            if (updatedGame == null || id <= 0 || id >= DBSize)
                return NoContent();

            bool result = _gameService.Update(id, updatedGame);
            
            if(!result)
                return NotFound();

            _logger.LogInformation("Update");
            return NoContent();
        }

        [HttpDelete("Delete/{id:int}")]
        public ActionResult Delete(int id)
        {
            if(id <= 0 || id >= DBSize)
                return NoContent();

            bool result = _gameService.Delete(id);

            if(!result)
                return NotFound();

            _logger.LogInformation("Delete");
            return NoContent();
        }

        private int DBSize => _gameService.GetAll().Count;
    }
}
