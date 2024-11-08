using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore.Design;


namespace DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        // DbSet properties representing tables in the database
        public DbSet<TemperatureSensor> TemperatureSensors { get; set; }
       // public DbSet<User> Users { get; set; }
       // public DbSet<PlantProfile> PlantProfiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Replace 'hydroponicsystem.db' with your desired database name or path
            optionsBuilder.UseSqlite("Data Source=hydroponicsystem.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure relationships, constraints, or additional configurations if needed
        }
    }
}
