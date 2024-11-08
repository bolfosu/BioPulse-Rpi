

namespace DataAccessLayer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public int PhoneNumber { get; set; }
        public ICollection<PlantProfile> PlantProfiles { get; set; }

    }
}
