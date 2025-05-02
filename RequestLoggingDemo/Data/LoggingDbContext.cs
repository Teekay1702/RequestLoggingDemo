using Microsoft.EntityFrameworkCore;
using RequestLoggingDemo.Models;

namespace RequestLoggingDemo.Data
{
    public class LoggingDbContext(DbContextOptions<LoggingDbContext> options) : DbContext(options)
    {
        public DbSet<RequestLog> RequestLogs { get; set; }
    }
}
