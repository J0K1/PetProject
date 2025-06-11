using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using PetProject.Models;
using PetProject.Services;
using System.Text.Json;

namespace PetProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : Controller
    {
        private readonly GameService _gameService;
        public GamesController (GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Game>> GetAll()
        {
            Console.WriteLine("Get all");
            return Ok(_gameService.GetAll());
        }

        //public async Task HandleGetAllGames(HttpContext context)
        //{
        //    var list = _gameService.GetAll();
        //    string parseJsonString = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        //    await context.Response.WriteAsync(parseJsonString);
        //}

        [HttpGet("{id}")]
        public ActionResult<Game> GetById(int id)
        {
            var game = _gameService.GetById(id);
            
            if(game == null)
                return NotFound();

            Console.WriteLine("Get by id");
            return Ok(game);
        }

        [HttpPost]
        public ActionResult<Game> Create(Game game)
        {
            _gameService.Add(game);
            Console.WriteLine("Create");
            return CreatedAtAction(nameof(GetById), new { id = game.Id }, game);
        }

        [HttpPut]
        public ActionResult<Game> Update(int id, Game updatedGame)
        {
            bool result = _gameService.Update(id, updatedGame);
            
            if(!result)
                return NotFound();

            Console.WriteLine("Update");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool result = _gameService.Delete(id);

            if(!result)
                return NotFound();

            Console.WriteLine("Delete");
            return NoContent();
        }
    }
}
