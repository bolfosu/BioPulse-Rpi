﻿namespace DataAccessLayer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswerHash { get; set; }
        public string? PhoneNumber { get; set; }


        

    }
}
