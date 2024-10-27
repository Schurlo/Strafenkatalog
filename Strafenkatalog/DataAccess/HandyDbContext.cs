using Strafenkatalog.Model;
using Strafenkatalog.Utilities;
using Microsoft.EntityFrameworkCore;


namespace Strafenkatalog.DataAccess
{
    public class HandyDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<FineType> FineTypes { get; set; }
        public DbSet<FineGiven> FinesGiven { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionDb = $"Filename={PathDB.GetPath("Test.db")}";
            optionsBuilder.UseSqlite(connectionDb);
        }
    }
}
