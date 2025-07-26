using System.ComponentModel.DataAnnotations;

namespace PetProject.Shared.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Login { get; set; } = string.Empty;
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Nick { get; set; } = string.Empty;
    }
}
