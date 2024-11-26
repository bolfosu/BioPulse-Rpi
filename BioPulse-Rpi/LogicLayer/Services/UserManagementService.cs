
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

        public async Task<bool> RegisterAsync(string name, string email, string password, string securityQuestion, string securityAnswer, string? phoneNumber = null)
        {
            if (await _userRepo.EmailExistsAsync(email))
                throw new InvalidOperationException("A user with this email already exists.");

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
            return true;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var hashedPassword = HashString(password);
            var user = await _userRepo.AuthenticateAsync(email, hashedPassword);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            return user;
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
