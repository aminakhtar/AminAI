using Microsoft.EntityFrameworkCore;
using AminAI.Server;

namespace AminAI.Server.Data
{
 public class WeatherDbContext : DbContext
 {
 public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options)
 {
 }

 public DbSet<AminAI.Server.WeatherForecast> WeatherForecasts { get; set; } = null!;

 protected override void OnModelCreating(ModelBuilder modelBuilder)
 {
 base.OnModelCreating(modelBuilder);

 // Use a lowercase table name to avoid PostgreSQL quoting issues
 modelBuilder.Entity<WeatherForecast>(entity =>
 {
 entity.ToTable("weatherforecasts");
 entity.HasKey(e => e.Id);
 });
 }
 }
}
