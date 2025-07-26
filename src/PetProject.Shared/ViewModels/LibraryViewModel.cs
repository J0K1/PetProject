using PetProject.Shared.Entities;

namespace PetProject.Shared.ViewModels
{
    public class LibraryViewModel
    {
        public List<GameEntity> Games { get; set; }
        public GameEntity SelectedGame { get; set; }

    }
}
