    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sentinel.Web.Sts.DbContexts;
    using Sentinel.Web.Sts.Models;

    namespace Sentinel.Web.Sts.Repositories
    {
        public class TokenRepository : ITokenRepository
        {
            private TokenDbContext dbContext;
            public TokenRepository(TokenDbContext dbContext)
            {
                this.dbContext = dbContext;
            }
            public bool AddToken(TokenRepoModel token)
            {
                dbContext.Tokens.Add(token);
                return dbContext.SaveChanges() > 0;
            }
            public bool ExpireToken(TokenRepoModel token)
            {
                dbContext.Tokens.Update(token);
                    return dbContext.SaveChanges() > 0;
            }
            public TokenRepoModel GetToken(string refresh_token, string client_id)
            {
                    return dbContext.Tokens.FirstOrDefault(x => x.ClientId == client_id && x.RefreshToken == refresh_token);
            }
        }
    }
