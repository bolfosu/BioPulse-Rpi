namespace DataAccessLayer.Models
{
    public class User
    {
        public int Id { get; set; }

        // User's name and email
        public string Name { get; set; }
        public string? Email { get; set; }

        // Secure password storage
        public string PasswordHash { get; set; }

        // Security question and answer for recovery
        public string SecurityQuestion { get; set; }
        public string SecurityAnswerHash { get; set; }

        // Optional phone number
        public string? PhoneNumber { get; set; }

        // Navigation property for associated plant profiles
        public virtual ICollection<PlantProfile> PlantProfiles { get; set; }
    }
}
