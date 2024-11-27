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

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="name">The user's name.</param>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="securityQuestion">Security question for password recovery.</param>
        /// <param name="securityAnswer">Answer to the security question.</param>
        /// <param name="phoneNumber">Optional phone number.</param>
        /// <returns>True if registration is successful.</returns>
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

        /// <summary>
        /// Authenticates a user by email and password.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The authenticated user object.</returns>
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

        /// <summary>
        /// Hashes a given string using SHA256.
        /// </summary>
        /// <param name="input">The string to hash.</param>
        /// <returns>The hashed string in Base64 format.</returns>
        private string HashString(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
