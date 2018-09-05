using System;
using Mercan.Common.Repos;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sentinel.Web.Model.Product;
using Sentinel.Web.Repos.Sql;

namespace Sentinel.Web.Repos.Repositories
{
    public class ProductRepo : EFRepository<ProductInfo>
    {

        public ProductRepo(SentinelDbContext db, ILogger logger = null) : base(db, p => p.Id, logger)
        {

        }
    }
}
