using Mercan.Common.Security;

namespace Mercan.Common.Mail
{
    public class MailServiceSettings : IMailServiceSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
