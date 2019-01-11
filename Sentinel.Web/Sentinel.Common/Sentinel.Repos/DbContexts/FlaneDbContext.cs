using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sentinel.Model.Product;
using System.Collections.Generic;


namespace Sentinel.Repos.Sql
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
                    ProductCode = "40008A",
                    Name = "Updating your Database Skills to Microsoft SQL Server 2012",
                    ProductUrl = "updating-your-database-skills-to-microsoft-sql-server-2012",
                    Active = true,
                    Html = "html",
                    DescriptionHtml = "DescriptionHtml",
                    ObjectivesHtml = "ObjectivesHtml",
                    AudienceHtml = "AudienceHtml",
                    PrerequisitesHtml = "PrerequisitesHtml",
                    TopicsHtml = "TopicsHtml",
                    RelatedHtml = "RelatedHtml",
                    RoadmapsHtml = "RoadmapsHtml",
                    Duration = "3",
                    DurationType = "Day",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 1,
                    TechnologyName = "SQL Server",
                    TechnologyUrl = "sql-server",
                    VendorId = 1,
                    VendorName = "Microsoft",
                    VendorUrl = "microsoft",
                    UseTabs = true
                },
                new
                {
                    Id = 2,
                    ProductCode = "40009A",
                    Name = "Updating your Business Intelligence Skills to Microsoft SQL Server 2012",
                    ProductUrl = "sql-server-updating-your-business-intelligence-skills-to-microsoft-sql-server-2012",
                    Active = true,
                    Html = "html",
                    DescriptionHtml = "DescriptionHtml",
                    ObjectivesHtml = "ObjectivesHtml",
                    AudienceHtml = "AudienceHtml",
                    PrerequisitesHtml = "PrerequisitesHtml",
                    TopicsHtml = "TopicsHtml",
                    RelatedHtml = "RelatedHtml",
                    RoadmapsHtml = "RoadmapsHtml",
                    Duration = "3",
                    DurationType = "Day",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 1,
                    TechnologyName = "SQL Server",
                    TechnologyUrl = "sql-server",
                    VendorId = 1,
                    VendorName = "Microsoft",
                    VendorUrl = "microsoft",
                    UseTabs = true
                },
                new
                {
                    Id = 3,
                    ProductCode = "20411B",
                    Name = "Administering Windows Server 2012",
                    ProductUrl = "administering-windows-server-2012",
                    Active = true,
                    Html = "html",
                    DescriptionHtml = "DescriptionHtml",
                    ObjectivesHtml = "ObjectivesHtml",
                    AudienceHtml = "AudienceHtml",
                    PrerequisitesHtml = "PrerequisitesHtml",
                    TopicsHtml = "TopicsHtml",
                    RelatedHtml = "RelatedHtml",
                    RoadmapsHtml = "RoadmapsHtml",
                    Duration = "5",
                    DurationType = "Day",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 2,
                    TechnologyName = "Windows Server",
                    TechnologyUrl = "windows-server",
                    VendorId = 1,
                    VendorName = "Microsoft",
                    VendorUrl = "microsoft",
                    UseTabs = true
                },
                new
                {
                    Id = 4,
                    ProductCode = "20412B",
                    Name = "Configuring Advanced Windows Server 2012",
                    ProductUrl = "configuring-advanced-windows-server-2012.html",
                    Active = true,
                    Html = "html",
                    DescriptionHtml = "DescriptionHtml",
                    ObjectivesHtml = "ObjectivesHtml",
                    AudienceHtml = "AudienceHtml",
                    PrerequisitesHtml = "PrerequisitesHtml",
                    TopicsHtml = "TopicsHtml",
                    RelatedHtml = "RelatedHtml",
                    RoadmapsHtml = "RoadmapsHtml",
                    Duration = "5",
                    DurationType = "Day",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 2,
                    TechnologyName = "Windows Server",
                    TechnologyUrl = "windows-server",
                    VendorId = 1,
                    VendorName = "Microsoft",
                    VendorUrl = "microsoft",
                    UseTabs = true
                },
                new
                {
                    Id = 5,
                    ProductCode = "20480B",
                    Name = "Programming in HTML5 with JavaScript and CSS3",
                    ProductUrl = "programming-in-html5-with-javascript-and-css3.html",
                    Active = true,
                    Html = "html",
                    DescriptionHtml = "DescriptionHtml",
                    ObjectivesHtml = "ObjectivesHtml",
                    AudienceHtml = "AudienceHtml",
                    PrerequisitesHtml = "PrerequisitesHtml",
                    TopicsHtml = "TopicsHtml",
                    RelatedHtml = "RelatedHtml",
                    RoadmapsHtml = "RoadmapsHtml",
                    Duration = "5",
                    DurationType = "Day",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 2,
                    TechnologyName = "Visual Studio",
                    TechnologyUrl = "visual-studio",
                    VendorId = 1,
                    VendorName = "Microsoft",
                    VendorUrl = "microsoft",
                    UseTabs = true
                },
                new
                {
                    Id = 6,
                    ProductCode = "20481B",
                    Name = "Essentials of developing Windows Metro style apps using HTML5 and JavaScript",
                    ProductUrl = "essentials-of-developing-windows-metro-style-apps-using-html5-and-javascript",
                    Active = true,
                    Html = "html",
                    DescriptionHtml = "DescriptionHtml",
                    ObjectivesHtml = "ObjectivesHtml",
                    AudienceHtml = "AudienceHtml",
                    PrerequisitesHtml = "PrerequisitesHtml",
                    TopicsHtml = "TopicsHtml",
                    RelatedHtml = "RelatedHtml",
                    RoadmapsHtml = "RoadmapsHtml",
                    Duration = "5",
                    DurationType = "Day",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 2,
                    TechnologyName = "Visual Studio",
                    TechnologyUrl = "visual-studio",
                    VendorId = 1,
                    VendorName = "Microsoft",
                    VendorUrl = "microsoft",
                    UseTabs = true
                },
                new
                {
                    Id = 7,
                    ProductCode = "20483B",
                    Name = "	Programming in C#",
                    ProductUrl = "essentials-of-developing-windows-metro-style-apps-using-html5-and-javascript",
                    Active = true,
                    Html = "html",
                    DescriptionHtml = "DescriptionHtml",
                    ObjectivesHtml = "ObjectivesHtml",
                    AudienceHtml = "AudienceHtml",
                    PrerequisitesHtml = "PrerequisitesHtml",
                    TopicsHtml = "TopicsHtml",
                    RelatedHtml = "RelatedHtml",
                    RoadmapsHtml = "RoadmapsHtml",
                    Duration = "5",
                    DurationType = "Day",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 2,
                    TechnologyName = "Visual Studio",
                    TechnologyUrl = "visual-studio",
                    VendorId = 1,
                    VendorName = "Microsoft",
                    VendorUrl = "microsoft",
                    UseTabs = true
                }, new
                {

                    Id = 8,
                    ProductCode = "40008A",
                    Name = "Updating your Database Skills to Microsoft SQL Server 2012",
                    ProductUrl = "updating-your-database-skills-to-microsoft-sql-server-2012",
                    Active = true,
                    Html = "html",
                    DescriptionHtml = "DescriptionHtml",
                    ObjectivesHtml = "ObjectivesHtml",
                    AudienceHtml = "AudienceHtml",
                    PrerequisitesHtml = "PrerequisitesHtml",
                    TopicsHtml = "TopicsHtml",
                    RelatedHtml = "RelatedHtml",
                    RoadmapsHtml = "RoadmapsHtml",
                    Duration = "3",
                    DurationType = "Day",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 1,
                    TechnologyName = "SQL Server",
                    TechnologyUrl = "sql-server",
                    VendorId = 1,
                    VendorName = "Microsoft",
                    VendorUrl = "microsoft",
                    UseTabs = true
                },
                new
                {
                    Id = 9,
                    ProductCode = "40009A",
                    Name = "Updating your Business Intelligence Skills to Microsoft SQL Server 2012",
                    ProductUrl = "sql-server-updating-your-business-intelligence-skills-to-microsoft-sql-server-2012",
                    Active = true,
                    Html = "html",
                    DescriptionHtml = "DescriptionHtml",
                    ObjectivesHtml = "ObjectivesHtml",
                    AudienceHtml = "AudienceHtml",
                    PrerequisitesHtml = "PrerequisitesHtml",
                    TopicsHtml = "TopicsHtml",
                    RelatedHtml = "RelatedHtml",
                    RoadmapsHtml = "RoadmapsHtml",
                    Duration = "3",
                    DurationType = "Day",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 1,
                    TechnologyName = "SQL Server",
                    TechnologyUrl = "sql-server",
                    VendorId = 1,
                    VendorName = "Microsoft",
                    VendorUrl = "microsoft",
                    UseTabs = true
                },
                new
                {
                    Id = 10,
                    ProductCode = "20411B",
                    Name = "Administering Windows Server 2012",
                    ProductUrl = "administering-windows-server-2012",
                    Active = true,
                    Html = "html",
                    DescriptionHtml = "DescriptionHtml",
                    ObjectivesHtml = "ObjectivesHtml",
                    AudienceHtml = "AudienceHtml",
                    PrerequisitesHtml = "PrerequisitesHtml",
                    TopicsHtml = "TopicsHtml",
                    RelatedHtml = "RelatedHtml",
                    RoadmapsHtml = "RoadmapsHtml",
                    Duration = "5",
                    DurationType = "Day",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 2,
                    TechnologyName = "Windows Server",
                    TechnologyUrl = "windows-server",
                    VendorId = 1,
                    VendorName = "Microsoft",
                    VendorUrl = "microsoft",
                    UseTabs = true
                },
                new
                {
                    Id = 11,
                    ProductCode = "20412B",
                    Name = "Configuring Advanced Windows Server 2012",
                    ProductUrl = "configuring-advanced-windows-server-2012.html",
                    Active = true,
                    Html = "html",
                    DescriptionHtml = "DescriptionHtml",
                    ObjectivesHtml = "ObjectivesHtml",
                    AudienceHtml = "AudienceHtml",
                    PrerequisitesHtml = "PrerequisitesHtml",
                    TopicsHtml = "TopicsHtml",
                    RelatedHtml = "RelatedHtml",
                    RoadmapsHtml = "RoadmapsHtml",
                    Duration = "5",
                    DurationType = "Day",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 2,
                    TechnologyName = "Windows Server",
                    TechnologyUrl = "windows-server",
                    VendorId = 1,
                    VendorName = "Microsoft",
                    VendorUrl = "microsoft",
                    UseTabs = true
                },
                new
                {
                    Id = 12,
                    ProductCode = "20480B",
                    Name = "Programming in HTML5 with JavaScript and CSS3",
                    ProductUrl = "programming-in-html5-with-javascript-and-css3.html",
                    Active = true,
                    Html = "html",
                    DescriptionHtml = "DescriptionHtml",
                    ObjectivesHtml = "ObjectivesHtml",
                    AudienceHtml = "AudienceHtml",
                    PrerequisitesHtml = "PrerequisitesHtml",
                    TopicsHtml = "TopicsHtml",
                    RelatedHtml = "RelatedHtml",
                    RoadmapsHtml = "RoadmapsHtml",
                    Duration = "5",
                    DurationType = "Day",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 2,
                    TechnologyName = "Visual Studio",
                    TechnologyUrl = "visual-studio",
                    VendorId = 1,
                    VendorName = "Microsoft",
                    VendorUrl = "microsoft",
                    UseTabs = true
                },
                new
                {
                    Id = 13,
                    ProductCode = "20481B",
                    Name = "Essentials of developing Windows Metro style apps using HTML5 and JavaScript",
                    ProductUrl = "essentials-of-developing-windows-metro-style-apps-using-html5-and-javascript",
                    Active = true,
                    Html = "html",
                    DescriptionHtml = "DescriptionHtml",
                    ObjectivesHtml = "ObjectivesHtml",
                    AudienceHtml = "AudienceHtml",
                    PrerequisitesHtml = "PrerequisitesHtml",
                    TopicsHtml = "TopicsHtml",
                    RelatedHtml = "RelatedHtml",
                    RoadmapsHtml = "RoadmapsHtml",
                    Duration = "5",
                    DurationType = "Day",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 2,
                    TechnologyName = "Visual Studio",
                    TechnologyUrl = "visual-studio",
                    VendorId = 2,
                    VendorName = "Cisco",
                    VendorUrl = "cisco",
                    UseTabs = true
                },
                new
                {
                    Id = 14,
                    ProductCode = "20483B",
                    Name = "	Programming in C#",
                    ProductUrl = "essentials-of-developing-windows-metro-style-apps-using-html5-and-javascript",
                    Active = true,
                    Html = "html",
                    DescriptionHtml = "DescriptionHtml",
                    ObjectivesHtml = "ObjectivesHtml",
                    AudienceHtml = "AudienceHtml",
                    PrerequisitesHtml = "PrerequisitesHtml",
                    TopicsHtml = "TopicsHtml",
                    RelatedHtml = "RelatedHtml",
                    RoadmapsHtml = "RoadmapsHtml",
                    Duration = "5",
                    DurationType = "Day",
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    TechnologyId = 2,
                    TechnologyName = "Visual Studio",
                    TechnologyUrl = "visual-studio",
                    VendorId = 2,
                    VendorName = "Cisco",
                    VendorUrl = "cisco",
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

