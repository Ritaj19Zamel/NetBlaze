using Microsoft.Extensions.Configuration;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.Email;
using NetBlaze.SharedKernel.HelperUtilities.General;
using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration;

    public EmailService(IConfiguration configuration)
    {
        _emailConfiguration = configuration
            .GetSection(nameof(EmailConfiguration))
            .Get<EmailConfiguration>()!;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var message = BuildMessage(to, subject, body);
        using var client = BuildClient();
        await client.SendMailAsync(message);
    }

    public async Task SendBulkAsync(IEnumerable<EmailMessageDto> messages)
    {
        using var client = BuildClient();

        foreach (var mail in messages)
        {
            var message = BuildMessage(mail.To, mail.Subject, mail.Body);
            await client.SendMailAsync(message);
        }
    }

    private MailMessage BuildMessage(string to, string subject, string body)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_emailConfiguration.Email),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(to);
        return message;
    }

    private SmtpClient BuildClient()
    {
        return new SmtpClient(_emailConfiguration.Host, _emailConfiguration.Port)
        {
            Credentials = new NetworkCredential(
                _emailConfiguration.Email,
                _emailConfiguration.Password),
            EnableSsl = _emailConfiguration.EnableSsl
        };
    }
}
