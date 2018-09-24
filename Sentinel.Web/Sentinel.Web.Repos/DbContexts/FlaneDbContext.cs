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
            });

            builder.Entity<ProductInfo>().HasData(
                new
                {
                    Id = 1,
                    ProductCode = "string",
                    Name = "string",
                    ProductUrl = "string",
                    Active = true,
                    Html = "string",
                    DescriptionHtml = "string",
                    ObjectivesHtml = "string",
                    AudienceHtml = "string",
                    PrerequisitesHtml = "string",
                    TopicsHtml = "string",
                    RelatedHtml = "string",
                    RoadmapsHtml = "string",
                    Duration = "string",
                    DurationType = "string",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 0,
                    TechnologyName = "string",
                    TechnologyUrl = "string",
                    VendorId = 0,
                    VendorName = 0,
                    VendorUrl = "string",
                    UseTabs = true
                },
                new
                {
                    Id = 2,
                    ProductCode = "string",
                    Name = "string",
                    ProductUrl = "string",
                    Active = true,
                    Html = "string",
                    DescriptionHtml = "string",
                    ObjectivesHtml = "string",
                    AudienceHtml = "string",
                    PrerequisitesHtml = "string",
                    TopicsHtml = "string",
                    RelatedHtml = "string",
                    RoadmapsHtml = "string",
                    Duration = "string",
                    DurationType = "string",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 0,
                    TechnologyName = "string",
                    TechnologyUrl = "string",
                    VendorId = 0,
                    VendorName = 0,
                    VendorUrl = "string",
                    UseTabs = true
                }
    // new { BlogId = 1, PostId = 2, Title = "Second post", Content = "Test 2" }
    );

            //.HasIndex(e=>e.Id).IsUnique();
            base.OnModelCreating(builder);
            logger.LogInformation("Seed Added");


        }
    }
}

