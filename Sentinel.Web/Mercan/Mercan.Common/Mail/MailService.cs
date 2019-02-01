using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Logging;

namespace Mercan.Common.Mail
{
    public class MailService
    {
        IOptions<MailServiceSettings> options;
        private ILogger<MailService> logger;

        public MailService(IOptions<MailServiceSettings> options, ILogger<MailService> logger)
        {
            this.options = options;
            this.logger = logger;
        }

        public void Send(string to, string subject, string body)
        {

            if (options != null && options.Value != null && options.Value.Server != null)
            {
                logger.LogDebug("mail server : " + options.Value.Server);
                var client = new SmtpClient(options.Value.Server, this.options.Value.Port);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(this.options.Value.UserName, this.options.Value.Password);
                // client.EnableSsl = true;

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(this.options.Value.UserName);
                mailMessage.To.Add(to);
                mailMessage.Body = body;
                mailMessage.Subject = subject;

                client.Send(mailMessage);
            }
            else
            {
                logger.LogDebug("Error on Mail Server options");
                if (options == null)
                {
                    logger.LogDebug("options Null");
                }
                else if (options.Value == null)
                {
                    logger.LogDebug("options.Value Null");
                }

                if (options.Value.Server == null)
                {
                    logger.LogDebug("options.Value.Server Null");

                }

                if (options.Value.Port == null)
                {
                    logger.LogDebug("options.Value.Port Null");
                }

                if (options.Value.UserName == null)
                {
                    logger.LogDebug("options.Value.Port Null");

                }

                if (options.Value.Password == null)
                {
                    logger.LogDebug("options.Value.Port Null");

                }

                else
                {
                    logger.LogDebug("I don't know what is Null ");
                }

            }
        }



    }

}