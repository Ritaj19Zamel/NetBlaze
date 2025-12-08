
namespace NetBlaze.Application.Interfaces.General
{
    public interface IUserContext
    {
        long UserId { get; }
        string Email { get; }
        string UserName { get; }
        List<string> Roles { get; }
        bool IsAuthenticated { get; }
    }
}
