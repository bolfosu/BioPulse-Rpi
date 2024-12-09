using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;

namespace LogicLayer.Services
{
    public class UserManagementService
    {
        private readonly UserRepo _userRepo;

        public UserManagementService(UserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        
        public async Task<bool> RegisterAsync(string name, string email, string password, string securityQuestion, string securityAnswer, string? phoneNumber = null)
        {
            Console.WriteLine($"Registering user: {name}, {email}, {phoneNumber}");
            if (await _userRepo.EmailExistsAsync(email))
            {
                Console.WriteLine($"Registration failed: A user with email {email} already exists.");
                throw new InvalidOperationException("A user with this email already exists.");
            }

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = HashString(password),
                SecurityQuestion = securityQuestion,
                SecurityAnswerHash = HashString(securityAnswer),
                PhoneNumber = phoneNumber
            };

            await _userRepo.AddAsync(user);
            Console.WriteLine($"User {email} added successfully.");
            return true;
        }

        
        public async Task<User> AuthenticateAsync(string email, string password)
        {
            Console.WriteLine($"Authenticating user: {email}");
            var hashedPassword = HashString(password);
            var user = await _userRepo.AuthenticateAsync(email, hashedPassword);

            if (user == null)
            {
                Console.WriteLine("Authentication failed: Invalid email or password.");
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            Console.WriteLine($"Authentication successful for: {email}");
            return user;
        }

        public async Task RecoverPasswordAsync(string email, string securityQuestion, string securityAnswer, string newPassword)
        {
            var user = await _userRepo.GetByEmailAsync(email);

            if (user == null)
                throw new InvalidOperationException("Email not found.");

            if (user.SecurityQuestion != securityQuestion || user.SecurityAnswerHash != HashString(securityAnswer))
                throw new UnauthorizedAccessException("Security question or answer is incorrect.");

            user.PasswordHash = HashString(newPassword);
            await _userRepo.UpdateAsync(user);
        }

        public async Task<string?> GetSecurityQuestionAsync(string email)
        {
            var user = await _userRepo.GetByEmailAsync(email);
            if (user == null)
                return null; // User not found

            return user.SecurityQuestion;
        }

        public async Task UpdateUserSettingsAsync(User user)
        {
            var existingUser = await _userRepo.GetByIdAsync(user.Id);
            if (existingUser == null)
                throw new InvalidOperationException("User not found.");

            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash; // Ensure it's already hashed
            existingUser.SecurityQuestion = user.SecurityQuestion;
            existingUser.SecurityAnswerHash = user.SecurityAnswerHash;

            // Notification Settings
            existingUser.IsWaterLevelLowNotificationEnabled = user.IsWaterLevelLowNotificationEnabled;
            existingUser.IsSensorNotChangingNotificationEnabled = user.IsSensorNotChangingNotificationEnabled;
            existingUser.IsSensorOffNotificationEnabled = user.IsSensorOffNotificationEnabled;

            await _userRepo.UpdateAsync(existingUser);
        }



        private string HashString(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
