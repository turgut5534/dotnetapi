using Forecast.Models;
using Microsoft.EntityFrameworkCore;

namespace Forecast.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { 
             
        }
        public DbSet<WeatherLocation> WeatherLocations { get; set; }
        public DbSet<Weather> Weathers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Weather>()
            .HasOne(w => w.WeatherLocation)
            .WithMany(wl => wl.Weathers)
            .HasForeignKey(w => w.WeatherLocationId);
        }
    }
}
