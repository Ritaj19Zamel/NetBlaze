namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
    }
}
