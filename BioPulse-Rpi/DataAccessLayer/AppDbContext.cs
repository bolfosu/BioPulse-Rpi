using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PlantProfile> PlantProfiles { get; set; }
    public DbSet<ImageCapture> ImageCaptures { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Use relative path to point to the database file
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"C:\Users\mobo\source\repos\BioPulse-Rpi\BioPulse-Rpi\DataAccessLayer\hydroponicsystem.db");
            var fullPath = Path.GetFullPath(dbPath);

            // Ensure the directory exists
            var directory = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Configure SQLite database with the resolved path
            optionsBuilder.UseSqlite($"Data Source={fullPath}");

            // Log for debugging purposes
            Console.WriteLine($"[AppDbContext] Database file path: {fullPath}");
        }
    }





    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Saving changes to database...");
        var result = await base.SaveChangesAsync(cancellationToken);
        Console.WriteLine($"Changes saved: {result} row(s) affected.");
        return result;
    }

}
