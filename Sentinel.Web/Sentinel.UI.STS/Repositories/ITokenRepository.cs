using Sentinel.UI.STS.Models;
namespace Sentinel.UI.STS.Repositories
{
    public interface ITokenRepository
    {
        bool AddToken(TokenRepoModel token);
        bool ExpireToken(TokenRepoModel token);
        TokenRepoModel GetToken(string refresh_token, string client_id);
    }
}
