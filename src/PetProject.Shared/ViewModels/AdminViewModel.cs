using PetProject.Shared.DTOs;
using PetProject.Shared.Entities;
namespace PetProject.Shared.ViewModels
{
    public class AdminViewModel
    {
        public List<UserEntity> Users { get; set; } = new();

        public List<GameEntity> Games { get; set; } = new();
    }
}
