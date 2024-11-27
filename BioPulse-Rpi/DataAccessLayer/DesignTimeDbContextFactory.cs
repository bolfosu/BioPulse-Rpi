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

            // Use the same database file path as in AppDbContext
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\DataAccessLayer\hydroponicsystem.db");
            var fullPath = Path.GetFullPath(dbPath);

            optionsBuilder.UseSqlite($"Data Source={fullPath}");

            // Log the database path for debugging purposes
            Console.WriteLine($"[DesignTimeDbContextFactory] Database file path: {fullPath}");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
