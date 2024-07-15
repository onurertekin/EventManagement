using DatabaseModel;
using DomainService.Config;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace DomainService.Operations
{
    public class EMailOperations
    {
        private readonly MainDbContext mainDbContext;
        private readonly SmtpConfig smtpConfig;

        public EMailOperations(MainDbContext mainDbContext, IOptions<SmtpConfig> smtpConfig)
        {
            this.mainDbContext = mainDbContext;
            this.smtpConfig = smtpConfig.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(smtpConfig.Host)
                {
                    Port = smtpConfig.Port,
                    Credentials = new NetworkCredential(smtpConfig.Username, smtpConfig.Password),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpConfig.From),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{to} adresine mail gönderirken hata oluştu!");
            }
        }
    }
}
