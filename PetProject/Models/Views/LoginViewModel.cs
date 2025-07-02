using System.ComponentModel.DataAnnotations;
using System.Runtime.Versioning;

namespace PetProject.Models.Views
{
    public class LoginViewModel
    {
        [Required]
        public string Login {  get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
