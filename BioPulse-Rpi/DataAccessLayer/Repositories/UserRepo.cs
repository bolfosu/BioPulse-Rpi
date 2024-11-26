
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class UserRepo : GenericRepository<User>
    {
        public UserRepo(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await GetDbSet().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> AuthenticateAsync(string email, string passwordHash)
        {
            return await GetDbSet().FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == passwordHash);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await GetDbSet().AnyAsync(u => u.Email == email);
        }
    }

}

