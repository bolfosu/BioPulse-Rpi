using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
    public class PlantProfileRepo : GenericRepository<PlantProfile>
    {
        public PlantProfileRepo(AppDbContext context) : base(context)
        {
        }

        
    }
}
