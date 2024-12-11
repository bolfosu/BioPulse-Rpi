namespace PresentationTier.DTOs.UserDTOs;

public class RecoverPasswordDto
{
    public string Email { get; set; }
    public string SecurityQuestion { get; set; }
    public string SecurityAnswer { get; set; }
    public string NewPassword { get; set; }
}