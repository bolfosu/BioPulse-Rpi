
﻿using DataAccessLayer.Models;

﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> LoginAsync(string email, string password)
        {
            return await _userRepo.CheckCredentialsAsync(email, password);
        }

        public async Task<bool> RegisterAsync(User user)
        {
            // Check if the email already exists
            var existingUser = await _userRepo.GetByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return false; // Registration failed, email already exists
            }

            await _userRepo.AddAsync(user);
            return true; // Registration successful
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepo.GetByEmailAsync(email);
        }

        private readonly UserRepo _userRepository;

       

        // Register a new user
        public async Task RegisterUserAsync(string name, string email, string password, string securityQuestion, string securityAnswer, string? phoneNumber = null)
        {
            if (await _userRepository.GetByEmailAsync(email) != null)
                throw new InvalidOperationException("A user with this email already exists.");

            var passwordHash = HashString(password);
            var securityAnswerHash = HashString(securityAnswer);

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = passwordHash,
                SecurityQuestion = securityQuestion,
                SecurityAnswerHash = securityAnswerHash,
                PhoneNumber = phoneNumber,
                PlantProfiles = new List<PlantProfile>()
            };

            await _userRepository.AddAsync(user);
        }

        // Authenticate user credentials
        public async Task<User> AuthenticateUserAsync(string email, string password)
        {
            var passwordHash = HashString(password);
            var user = await _userRepository.AuthenticateAsync(email, passwordHash);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            return user;
        }

        // Recover account by verifying security answer
        public async Task<bool> RecoverAccountAsync(string email, string securityAnswer)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var securityAnswerHash = HashString(securityAnswer);
            return user.SecurityAnswerHash == securityAnswerHash;
        }

        // Update user profile
        public async Task UpdateUserProfileAsync(int userId, string name, string? phoneNumber = null)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            user.Name = name;
            user.PhoneNumber = phoneNumber;
            await _userRepository.UpdateAsync(user);
        }

        // Delete a user
        public async Task DeleteUserAsync(int userId)
        {
            await _userRepository.DeleteAsync(userId);
        }

        // List all users
        public async Task<IEnumerable<User>> ListAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        // Helper: Hash strings
        private string HashString(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);

        }
    }
}
