using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
    public class PhSensorRepo : GenericRepository<PhSensor>
    {
        public PhSensorRepo(AppDbContext context) : base(context)
        {
        }

        
    }
}
