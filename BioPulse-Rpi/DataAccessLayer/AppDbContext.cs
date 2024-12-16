using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets for each entity
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<SensorReading> SensorReadings { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PlantProfile> PlantProfiles { get; set; }
    public DbSet<ImageCapture> ImageCaptures { get; set; }
    public DbSet<Actuator> Actuators { get; set; } // Add Actuators

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Sensor and SensorReading relationship
        modelBuilder.Entity<Sensor>()
            .HasMany(s => s.SensorReadings)
            .WithOne(sr => sr.Sensor)
            .HasForeignKey(sr => sr.SensorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Sensor>()
            .HasIndex(s => s.SensorType);

        modelBuilder.Entity<SensorReading>()
            .HasIndex(sr => sr.Timestamp);

        // Configure User table
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Configure PlantProfile
        modelBuilder.Entity<PlantProfile>()
            .Property(p => p.Name)
            .IsRequired();

        // Configure Actuator table
        modelBuilder.Entity<Actuator>()
            .Property(a => a.ActuatorType)
            .HasConversion<int>() // Enum to integer conversion
            .IsRequired();

        modelBuilder.Entity<Actuator>()
            .Property(a => a.IsOn)
            .IsRequired();

        modelBuilder.Entity<Actuator>()
            .Property(a => a.Name)
            .IsRequired();

        modelBuilder.Entity<Actuator>()
            .HasIndex(a => a.ActuatorType);
    }
}
