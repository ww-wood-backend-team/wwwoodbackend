using System;
using Microsoft.EntityFrameworkCore;
namespace wwwoodbackend.Models
{
    public class LogContext : DbContext
    {
        public LogContext(DbContextOptions<LogContext> options)
        : base(options)
        {
            this.Database.SetCommandTimeout(300);
        }

        public DbSet<Log> ApplicationLogs { get; set; }
    }
}
