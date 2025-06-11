using PetProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PetProject.Services
{
    public class GameService
    {
        private List<Game> games = new()
        {
            new Game { Id = 1, Title = "The Witcher 3", Genre = "RPG", Year = 2015 },
            new Game { Id = 2, Title = "Cyberpunk 2077", Genre = "RPG", Year = 2020 }
        };

        public List<Game> GetAll() => games;
        public Game? GetById(int id) => games.FirstOrDefault(g => g.Id == id);
        public void Add(Game game)
        {
            game.Id = games.Max(g => g.Id) + 1;
            games.Add(game);
        }
        public bool Update(int id, Game updated)
        {
            var game = GetById(id);
            if (game == null) return false;
            game.Title = updated.Title;
            game.Genre = updated.Genre;
            game.Year = updated.Year;
            return true;
        }
        public bool Delete(int id)
        {
            var game = GetById(id);
            if (game == null) return false;
            games.Remove(game);
            return true;
        }
    }
}
