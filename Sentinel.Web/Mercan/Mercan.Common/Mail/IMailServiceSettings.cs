using Mercan.Common.Security;

namespace Mercan.Common.Mail
{
    public interface IMailServiceSettings
    {
        string Server { get; }
        int Port { get; }
        string UserName { get; }
        string Password { get; }
    }
}
