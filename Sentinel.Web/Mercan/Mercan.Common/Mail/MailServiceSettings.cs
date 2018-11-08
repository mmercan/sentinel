using Mercan.Common.Security;

namespace Mercan.Common.Mail
{
    public class MailServiceSettings : IMailServiceSettings
    {
        public string Server { get; }
        public int Port { get; }
        public string UserName { get; }
        public string Password { get; }
    }
}
