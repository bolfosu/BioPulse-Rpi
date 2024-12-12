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

        public async Task<bool> UpdateUserSettingsAsync(int userId, string? newEmail, string? newPassword, string? newPhoneNumber, string? newSecurityQuestion, string? newSecurityAnswer)
        {
            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Check if a new email is provided and it's different from the current email
            if (!string.IsNullOrWhiteSpace(newEmail) && newEmail != user.Email)
            {
                if (await _userRepo.EmailExistsAsync(newEmail))
                {
                    throw new InvalidOperationException("Email already in use.");
                }
                user.Email = newEmail;
            }

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                user.PasswordHash = HashString(newPassword);
            }

            if (!string.IsNullOrWhiteSpace(newPhoneNumber))
            {
                user.PhoneNumber = newPhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(newSecurityQuestion) && !string.IsNullOrWhiteSpace(newSecurityAnswer))
            {
                user.SecurityQuestion = newSecurityQuestion;
                user.SecurityAnswerHash = HashString(newSecurityAnswer);
            }

            await _userRepo.UpdateAsync(user);
            return true;
        }





        private string HashString(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                Console.WriteLine($"No user found with ID: {id}");
                throw new KeyNotFoundException($"No user found with ID: {id}");
            }
            Console.WriteLine($"Retrieved user: {user.Name}, ID: {user.Id}");
            return user;
        }
    }
}