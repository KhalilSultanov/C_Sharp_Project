using System;
using Microsoft.EntityFrameworkCore;
using AvaloniaApplication.Views;

namespace AvaloniaApplication.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Movie> movies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine("OnConfiguring");
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=AvaloniaDB;Username=postgres;Password=khalil;");
        }
    }
}