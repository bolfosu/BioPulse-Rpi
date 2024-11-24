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
    }
}
