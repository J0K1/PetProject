using PetProject.Shared.DTOs;

public class UserDetailsDto : UserDto
{
    public List<UserDto> Friends { get; set; } = new();
    public List<GameDto> Games { get; set; } = new();
}
