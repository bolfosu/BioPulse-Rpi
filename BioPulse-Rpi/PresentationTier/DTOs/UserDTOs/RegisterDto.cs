namespace PresentationTier.DTOs.UserDTOs;

public class RegisterDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string SecurityQuestion { get; set; }
    public string SecurityAnswer { get; set; }
    public string? PhoneNumber { get; set; }
}