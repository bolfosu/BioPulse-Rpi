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
            // Use the database file in the DataAccessLayer directory
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\DataAccessLayer\hydroponicsystem.db");
            var fullPath = Path.GetFullPath(dbPath);

            optionsBuilder.UseSqlite($"Data Source={fullPath}");

            // Log the database path for debugging purposes
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
