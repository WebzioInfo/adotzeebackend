using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Adotzee_Backend.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly ILogger<SmtpEmailService> _logger;

        public SmtpEmailService(ILogger<SmtpEmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            try
            {
                // TODO: Load from configuration/environment variables
                var smtpHost = "smtp.example.com";
                var smtpPort = 587;
                var smtpUser = "user@example.com";
                var smtpPass = "password";

                var client = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("no-reply@adotzee.com", "Adotzee System"),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                // _logger.LogInformation($"Sending email to {toEmail}...");
                // await client.SendMailAsync(mailMessage);
                _logger.LogInformation($"Mock Email sent to {toEmail}: {subject}");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email to {toEmail}: {ex.Message}");
            }
        }
    }
}
