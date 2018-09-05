    using Microsoft.EntityFrameworkCore;
    using System.IO;
    using Sentinel.Web.Sts.Models;
    namespace Sentinel.Web.Sts.DbContexts
    {
        public class TokenDbContext : DbContext
        {
            public DbSet<TokenRepoModel> Tokens { get; set; }
            public TokenDbContext(DbContextOptions<TokenDbContext> options): base(options)
            { }
            //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            //{
            //    var connStr = Path.Combine(Directory.GetCurrentDirectory(), "demo.db");
            //    optionsBuilder.UseSqlite($"Data Source={connStr}");
            //}
        }
    }
