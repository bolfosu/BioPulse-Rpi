using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    public DbSet<TemperatureSensor> TemperatureSensors { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PlantProfile> PlantProfiles { get; set; }
    public DbSet<EcSensor> EcSensors { get; set; }
    public DbSet<PhSensor> PhSensors { get; set; }
    public DbSet<LightSensor> LightSensors { get; set; }
    public DbSet<ImageCapture> ImageCaptures { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var dbPath = "hydroponicsystem.db";
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            // Log the full path of the database
            var fullPath = Path.GetFullPath(dbPath);
            Console.WriteLine($"Database file path: {fullPath}");
        }
    }
}
