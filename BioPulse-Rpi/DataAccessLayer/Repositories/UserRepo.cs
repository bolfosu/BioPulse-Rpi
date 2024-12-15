using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepo : GenericRepository<User>
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public UserRepo(IDbContextFactory<AppDbContext> contextFactory) : base(contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> AuthenticateAsync(string email, string passwordHash)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == passwordHash);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            using var context = _contextFactory.CreateDbContext();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Users.FindAsync(id);
        }
    }
}
