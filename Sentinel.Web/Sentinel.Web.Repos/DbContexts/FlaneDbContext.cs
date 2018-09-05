using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sentinel.Web.Model.Product;
using System.Collections.Generic;


namespace Sentinel.Web.Repos.Sql
{
    public class SentinelDbContext : DbContext
    {

        ILogger<SentinelDbContext> logger;
        public DbSet<ProductInfo> ProductSet { get; set; }

        public SentinelDbContext(DbContextOptions options, ILogger<SentinelDbContext> logger) : base(options)
        {
            this.logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseLoggerFactory()
            //  optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProductInfo>(e =>
            {
                e.HasKey(en => en.Id);//.HasIndex(en => en.Id);
                e.Property(p => p.Id).ValueGeneratedOnAdd();
            });//.HasIndex(e=>e.Id).IsUnique();
            base.OnModelCreating(builder);



        }
    }
}

