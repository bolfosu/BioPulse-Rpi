
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class UserRepo : GenericRepository<User>
    {
        public UserRepo(AppDbContext context) : base(context) { }

        // Get user by email
        public async Task<User> GetByEmailAsync(string email)
        {
            return await GetDbSet().FirstOrDefaultAsync(u => u.Email == email);
        }

        // Authenticate user
        public async Task<User> AuthenticateAsync(string email, string passwordHash)
        {
            return await GetDbSet().FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == passwordHash);
        }
    }
}
