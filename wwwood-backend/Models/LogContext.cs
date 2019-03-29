using System;
using Microsoft.EntityFrameworkCore;
namespace wwwoodbackend.Models
{
    public class LogContext : DbContext
    {
        public LogContext(DbContextOptions<LogContext> options)
        : base(options)
        {
        }

        public DbSet<Log> Logs { get; set; }
    }
}
