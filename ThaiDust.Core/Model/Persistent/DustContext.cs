using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ThaiDust.Core.Model.Persistent
{
    public class DustContext : DbContext
    {
        private readonly string _databasePath;
        public DbSet<Station> Stations { get; set; }
        public DbSet<Record> Records { get; set; }

        public DustContext() { }

        public DustContext(string databasePath)
        {
            _databasePath = databasePath;
        }

        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder =>
            {
                builder.AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information).AddDebug();
            });

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={_databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Record>()
                .HasKey(o => new { o.DateTime, o.Type, o.StationCode });
        }
    }
}
