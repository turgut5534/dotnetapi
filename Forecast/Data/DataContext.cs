using Forecast.Models;
using Microsoft.EntityFrameworkCore;

namespace Forecast.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { 
             
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Weather> Weathers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.Password)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.HasMany(u => u.Locations)
                      .WithOne(l => l.User)
                      .HasForeignKey(l => l.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure the Location entity
            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(l => l.LocationName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(l => l.Latitude)
                      .IsRequired();

                entity.Property(l => l.Longitude)
                      .IsRequired();

                entity.HasMany(l => l.Forecasts)
                      .WithOne(f => f.Location)
                      .HasForeignKey(f => f.LocationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure the Forecast entity
            modelBuilder.Entity<Weather>(entity =>
            {
                entity.Property(f => f.Temperature)
                      .IsRequired();

                entity.Property(f => f.Humidity)
                      .IsRequired();

                entity.Property(f => f.WindSpeed)
                      .IsRequired();

                entity.Property(f => f.WindDirection)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(f => f.WeatherCondition)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(f => f.ForecastDate)
                      .IsRequired();

                entity.HasOne(f => f.Location)
                      .WithMany(l => l.Forecasts)
                      .HasForeignKey(f => f.LocationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
