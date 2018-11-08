using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Mercan.Common.Mail
{
    public class MailService
    {
        IOptions<MailServiceSettings> options;
        public MailService(IOptions<MailServiceSettings> options)
        {
            this.options = options;
        }

        public void SendAsync(string to, string subject, string body)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            //       client.UseDefaultCredentials = false;
            //       client.Credentials = new NetworkCredential("email@gmail.com", "some password");
            //       client.EnableSsl = true;

            //         MailMessage mailMessage = new MailMessage();
            //         mailMessage.From = new MailAddress("email@gmail.com");
            //         mailMessage.To.Add("otheremail@gmail.com");
            //         mailMessage.Body = "Hello World!";
            //         mailMessage.Subject = "Subjeeect";

            //         client.Send(mailMessage);
        }



    }

}