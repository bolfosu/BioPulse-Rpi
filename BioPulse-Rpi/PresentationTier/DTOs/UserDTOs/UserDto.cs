
namespace PresentationTier.DTOs.UserDTOs
{
    public class UserDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? SecurityQuestion { get; set; }
        public string? SecurityAnswer { get; set; }
        public string? PhoneNumber { get; set; }

        // Password Recovery
        public string? NewPassword { get; set; }

        // Email Update
        public string? NewEmail { get; set; }

        // Security Update
        public string? NewSecurityQuestion { get; set; }
        public string? NewSecurityAnswer { get; set; }
    }
}