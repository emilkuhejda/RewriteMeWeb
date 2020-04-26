using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class MailService : IMailService
    {
        private readonly ILogger _logger;
        private readonly AppSettings _appSettings;

        public MailService(
            ILogger logger,
            IOptions<AppSettings> options)
        {
            _logger = logger;
            _appSettings = options.Value;
        }

        public async Task SendAsync(string recipient, string subject, string body)
        {
            try
            {
                var mailConfiguration = _appSettings.MailConfiguration;
                using (var client = new SmtpClient(mailConfiguration.SmtpServer, mailConfiguration.Port))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(mailConfiguration.Username, mailConfiguration.Password);

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(mailConfiguration.From, mailConfiguration.DisplayName);
                        mailMessage.To.Add(recipient);
                        mailMessage.Body = body;
                        mailMessage.Subject = subject;
                        await client.SendMailAsync(mailMessage).ConfigureAwait(false);

                        _logger.Information($"Email was successfully sent to recipient: '{recipient}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Exception occurred during sending email.");
                _logger.Error(ExceptionFormatter.FormatException(ex));
            }
        }
    }
}
