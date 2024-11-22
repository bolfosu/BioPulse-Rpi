using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<TemperatureSensor> TemperatureSensors { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PlantProfile> PlantProfiles { get; set; }
    public DbSet<EcSensor> EcSensors { get; set; }
    public DbSet<PhSensor> PhSensors { get; set; }
    public DbSet<LightSensor> LightSensors { get; set; }
    public DbSet<ImageCapture> ImageCaptures { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=hydroponicsystem.db");
        }
    }
}
