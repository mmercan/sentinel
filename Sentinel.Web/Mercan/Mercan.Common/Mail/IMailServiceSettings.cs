using Mercan.Common.Security;

namespace Mercan.Common.Mail
{
    public interface IMailServiceSettings
    {
        string Server { get; set; }
        int Port { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}
