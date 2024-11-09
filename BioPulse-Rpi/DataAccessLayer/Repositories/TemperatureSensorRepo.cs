using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
    public class TemperatureSensorRepo : GenericRepository<TemperatureSensor>
    {
        
        public TemperatureSensorRepo(AppDbContext context) : base(context) 
        {
        }

        public async Task<IEnumerable<TemperatureSensor>> GetEnabledSensorsAsync()
        {
            return await GetDbSet().Where(sensor => sensor.IsEnabled).ToListAsync();
        }
    }
}