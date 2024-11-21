using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
    public class LightSensorRepo : GenericRepository<LightSensor>
    {
        public LightSensorRepo(AppDbContext context) : base(context)
        {
        }

     
    }
}
