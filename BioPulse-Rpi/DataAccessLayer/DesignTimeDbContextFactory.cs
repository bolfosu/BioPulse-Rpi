using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;

namespace DataAccessLayer
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Use relative path to ensure database is in the correct location
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"C:\Users\mobo\source\repos\BioPulse-Rpi\BioPulse-Rpi\DataAccessLayer\hydroponicsystem.db");
            var fullPath = Path.GetFullPath(dbPath);

            // Ensure the directory exists
            var directory = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Configure SQLite with the resolved path
            optionsBuilder.UseSqlite($"Data Source={fullPath}");

            // Log for debugging purposes
            Console.WriteLine($"[DesignTimeDbContextFactory] Database file path: {fullPath}");

            return new AppDbContext(optionsBuilder.Options);
        }


    }
}
