using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<TemperatureSensor> TemperatureSensors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PlantProfile> PlantProfiles { get; set; }
        public DbSet<EcSensor> EcSensors { get; set; }
        public DbSet<PhSensor> PhSensors { get; set; }
        public DbSet<LightSensor> LightSensors { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Only configure SQLite if not configured externally (e.g., for tests)
                optionsBuilder.UseSqlite("Data Source=hydroponicsystem.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure relationships, constraints, or additional configurations if needed
        }
    }
}
