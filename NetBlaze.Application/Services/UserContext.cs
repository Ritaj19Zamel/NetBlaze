
using Microsoft.AspNetCore.Http;
using NetBlaze.Application.Interfaces.General;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NetBlaze.Application.Services
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        public long UserId
        {
            get
            {
                var sid = _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sid);
                return string.IsNullOrEmpty(sid) ? 0 : long.Parse(sid);
            }
        }
        public string Email =>_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) ?? "";
        public string UserName =>_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        public List<string> Roles =>_httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList()?? [];
    }

}
