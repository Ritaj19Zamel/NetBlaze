using System.Security.Claims;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IPermissionService
    {
        Task<bool> UserHasPermissionAsync(string path, string method);

    }
}
