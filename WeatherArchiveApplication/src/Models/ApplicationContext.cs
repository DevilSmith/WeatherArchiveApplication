using Microsoft.EntityFrameworkCore;

namespace WeatherArchiveApp.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() { }

        public DbSet<WeatherRecord> WeatherRecords { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=weather_archive_app_postgres;Port=5432;Username=postgres;Password=123456789");
    }
}
