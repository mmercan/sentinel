using Sentinel.Web.Sts.Models;
namespace Sentinel.Web.Sts.Repositories
{
    public interface ITokenRepository
    {
        bool AddToken(TokenRepoModel token);
        bool ExpireToken(TokenRepoModel token);
        TokenRepoModel GetToken(string refresh_token, string client_id);
    }
}
