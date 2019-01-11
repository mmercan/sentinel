using Microsoft.EntityFrameworkCore;
using System.IO;
using Sentinel.UI.Sts.Models;
namespace Sentinel.UI.Sts.DbContexts
{
    public class TokenDbContext : DbContext
    {
        public DbSet<TokenRepoModel> Tokens { get; set; }
        public TokenDbContext(DbContextOptions<TokenDbContext> options) : base(options)
        { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var connStr = Path.Combine(Directory.GetCurrentDirectory(), "demo.db");
        //    optionsBuilder.UseSqlite($"Data Source={connStr}");
        //}
    }
}
