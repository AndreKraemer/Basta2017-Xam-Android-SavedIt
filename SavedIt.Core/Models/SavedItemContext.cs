using System.IO;
using Microsoft.EntityFrameworkCore;

namespace SavedIt.Core.Models
{
    public class SavedItemContext : DbContext
    {
        private readonly string _path;

        public SavedItemContext(string path)
        {
            _path = Path.Combine(path, "saveditems12.db");
        }
        public DbSet<SavedItem> SavedItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Filename={_path}");
        }
    }
}