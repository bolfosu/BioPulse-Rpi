using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<SensorReading> SensorReadings { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PlantProfile> PlantProfiles { get; set; }
    public DbSet<ImageCapture> ImageCaptures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Sensor>()
            .HasMany(s => s.SensorReadings)
            .WithOne(sr => sr.Sensor)
            .HasForeignKey(sr => sr.SensorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Sensor>()
            .HasIndex(s => s.SensorType);

        modelBuilder.Entity<SensorReading>()
            .HasIndex(sr => sr.Timestamp);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
