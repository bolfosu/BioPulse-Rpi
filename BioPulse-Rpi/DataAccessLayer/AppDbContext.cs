using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets for all entities
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<SensorReading> SensorReadings { get; set; } // Add SensorReading
    public DbSet<User> Users { get; set; }
    public DbSet<PlantProfile> PlantProfiles { get; set; }
    public DbSet<ImageCapture> ImageCaptures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Sensor and SensorReading relationship
        modelBuilder.Entity<SensorReading>()
            .HasOne(sr => sr.Sensor) // A SensorReading has one Sensor
            .WithMany(s => s.SensorReadings) // A Sensor has many SensorReadings
            .HasForeignKey(sr => sr.SensorId); // Foreign key in SensorReading

        // Optional: Add an index on SensorType for querying
        modelBuilder.Entity<Sensor>()
            .HasIndex(s => s.SensorType);

        // Configure SensorReading Timestamp indexing (optional for time-series performance)
        modelBuilder.Entity<SensorReading>()
            .HasIndex(sr => sr.Timestamp);
    }
}
