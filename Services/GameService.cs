using PetProject.Models;

namespace PetProject.Services
{
    public class GameService
    {
        private List<Game> gamesList = new()
        {
            new Game { Id = 1, Title = "The Witcher 3", Genre = "RPG", Year = 2015 },
            new Game { Id = 2, Title = "Cyberpunk 2077", Genre = "RPG", Year = 2020 },
            new Game { Id = 3, Title = "Red Dead Redemption 2", Genre = "Action-Adventure", Year = 2018 },
            new Game { Id = 4, Title = "Elden Ring", Genre = "Action RPG", Year = 2022 },
            new Game { Id = 5, Title = "God of War", Genre = "Action", Year = 2018 },
            new Game { Id = 6, Title = "Minecraft", Genre = "Sandbox", Year = 2011 },
            new Game { Id = 7, Title = "Hollow Knight", Genre = "Metroidvania", Year = 2017 },
            new Game { Id = 8, Title = "Stardew Valley", Genre = "Simulation", Year = 2016 },
            new Game { Id = 9, Title = "Portal 2", Genre = "Puzzle", Year = 2011 },
            new Game { Id = 10, Title = "DOOM Eternal", Genre = "Shooter", Year = 2020 }
        };

        public List<Game> GetAll() => gamesList;

        public Game? GetById(int id) => gamesList.FirstOrDefault(g => g.Id == id);

        public List<Game> GetByTitle(string title)
        {
            List<Game> games = gamesList
                .Where(g => g.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return games;
        }

        public List<Game> GetByYear(int year)
        {
            List<Game> games = gamesList.FindAll(g => g.Year == year);
            return games;
        }

        public void Add(Game game)
        {
            game.Id = gamesList.Max(g => g.Id) + 1;
            gamesList.Add(game);
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
            
            if (game == null) 
                return false;
            
            gamesList.Remove(game);
            return true;
        }
    }
}
