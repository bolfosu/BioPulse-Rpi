using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
    public class EcSensorRepo : GenericRepository<EcSensor>
    {
        public EcSensorRepo(AppDbContext context) : base(context)
        {
        }

       
    }
}
