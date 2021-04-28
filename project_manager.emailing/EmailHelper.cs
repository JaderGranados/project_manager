using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using project_manager.common.Models;

namespace project_manager.emailing
{
    public class EmailHelper
    {
        public static void SendEmail(object emailModel)
        {
            try
            {
                var model = emailModel as EmailModel;
                IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();
                string emailOrigen = configuration["EmailData:OriginEmail"];
                string host = configuration["EmailData:Host"];
                string password = configuration["EmailData:Password"];
                string port = configuration["EmailData:Port"];

                MailMessage mailMessage = new MailMessage(emailOrigen, model.Email, model.Subject, model.Body);
                mailMessage.IsBodyHtml = model.IsHtml;

                SmtpClient smtpClient = new SmtpClient(host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = int.Parse(port);
                smtpClient.Credentials = new NetworkCredential(emailOrigen, password);

                smtpClient.Send(mailMessage);

                smtpClient.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}


