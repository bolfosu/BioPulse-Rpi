using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore.Design;


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


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlite("Data Source=hydroponicsystem.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure relationships, constraints, or additional configurations if needed
        }
    }
}
