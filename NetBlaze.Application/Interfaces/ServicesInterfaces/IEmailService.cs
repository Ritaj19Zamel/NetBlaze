using NetBlaze.SharedKernel.Dtos.Email;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);

        Task SendBulkAsync(IEnumerable<EmailMessageDto> messages);
    }

}
