using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.HelperUtilities.General;
using System.Net;
using System.Net.Mail;

namespace NetBlaze.Application.Services
{
    internal class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(IConfiguration configuration)
        {
            _emailConfiguration = configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>()!;
        }
        public async Task SendAsync(string to, string subject, string body)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_emailConfiguration.Email),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(to);

            using var client = new SmtpClient(_emailConfiguration.Host, _emailConfiguration.Port)
            {
                Credentials = new NetworkCredential(_emailConfiguration.Email, _emailConfiguration.Password),
                EnableSsl = _emailConfiguration.EnableSsl
            };

            await client.SendMailAsync(message);
        }
    }
}
