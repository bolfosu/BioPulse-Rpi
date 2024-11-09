using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
    public class UserRepo : GenericRepository<User>
    {
        public UserRepo(AppDbContext context) : base(context)
        {
        }

        // You can add custom methods related to User if needed
        public async Task<User> GetByEmailAsync(string email)
        {
            return await GetDbSet().FirstOrDefaultAsync(user => user.Email == email);
        }
    }
}
