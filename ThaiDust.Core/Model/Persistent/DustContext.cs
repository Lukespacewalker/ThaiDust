using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ThaiDust.Core.Model.Persistent
{
    public class DustContext : DbContext
    {
        private readonly string _databasePath;
        public DbSet<Station> Stations { get; set; }
        public DbSet<Record> Records { get; set; }

        // public DustContext() { }

        public DustContext(string databasePath)
        {
            _databasePath = databasePath;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={_databasePath}");
        }
    }
}
