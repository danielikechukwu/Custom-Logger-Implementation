using Microsoft.EntityFrameworkCore;

namespace CustomLoggerImplementation.Models
{
    public class LoggingDbContext : DbContext
    {
        public LoggingDbContext(DbContextOptions<LoggingDbContext> options) : base(options)
        {
        }

        // A DbSet to hold our log entries
        public DbSet<LogEntry> LogEntries { get; set; }
    }
}
