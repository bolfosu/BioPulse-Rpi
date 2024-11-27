using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class UserRepo : GenericRepository<User>
    {
        public UserRepo(AppDbContext context) : base(context) { }

        /// <summary>
        /// Gets a user by their email address.
        /// </summary>
        /// <param name="email">The email to search for.</param>
        /// <returns>The user if found, otherwise null.</returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                Console.WriteLine($"Searching for user by email: {email}");
                var user = await GetDbSet().FirstOrDefaultAsync(u => u.Email == email);
                Console.WriteLine(user == null
                    ? $"No user found with email: {email}"
                    : $"User found: {user.Name}, {user.Email}");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetByEmailAsync: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Authenticates a user using their email and password hash.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="passwordHash">The hashed password.</param>
        /// <returns>The user if authentication is successful, otherwise null.</returns>
        public async Task<User?> AuthenticateAsync(string email, string passwordHash)
        {
            try
            {
                Console.WriteLine($"Authenticating user with email: {email}");
                var user = await GetDbSet().FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == passwordHash);
                Console.WriteLine(user == null
                    ? $"Authentication failed for email: {email}"
                    : $"Authentication successful for user: {user.Name}");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in AuthenticateAsync: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Checks if an email is already registered.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email exists, otherwise false.</returns>
        public async Task<bool> EmailExistsAsync(string email)
        {
            try
            {
                Console.WriteLine($"Checking if email exists: {email}");
                var exists = await GetDbSet().AnyAsync(u => u.Email == email);
                Console.WriteLine($"Email {email} exists: {exists}");
                return exists;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in EmailExistsAsync: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user to add.</param>
        public async Task AddAsync(User user)
        {
            try
            {
                Console.WriteLine($"Adding user: {user.Email}");
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                Console.WriteLine($"User {user.Email} added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in AddAsync: {ex.Message}");
                throw;
            }
        }
    }
}
