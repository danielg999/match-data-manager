using Microsoft.EntityFrameworkCore;

namespace MatchDataManager.Api.Entities
{
    public class MatchDataManagerDbContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Team> Teams { get; set; }

        public string DbPath { get; }

        public MatchDataManagerDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "MatchDataManagerDb.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}