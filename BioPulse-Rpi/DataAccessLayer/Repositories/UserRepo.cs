using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
    public class UserRepo : GenericRepository<User>
    {
        public UserRepo(AppDbContext context) : base(context)
        {
        }

        
        public async Task<User> GetByEmailAsync(string email)
        {
            return await GetDbSet().FirstOrDefaultAsync(user => user.Email == email);
        }
        //add, update, getbyid...
    }
}
