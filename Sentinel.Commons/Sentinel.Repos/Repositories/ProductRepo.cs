using System;
using Mercan.Common.Data.Repos;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sentinel.Model.Product;
using Sentinel.Repos.Sql;

namespace Sentinel.Repos.Repositories
{
    public class ProductRepo : EFRepository<ProductInfo>
    {

        public ProductRepo(SentinelDbContext db, ILogger logger = null) : base(db, p => p.Id, logger)
        {

            //Reqired to seed 
            try
            {
                db.Database.EnsureCreated();
                logger.LogInformation("Database is Ready");
            }
            catch (Exception ex)
            {
                logger.LogError("db.Database.EnsureCreated with Error", ex.Message);
            }
        }
    }
}
