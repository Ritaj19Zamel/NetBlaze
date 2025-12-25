using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;

namespace NetBlaze.Api.Filters
{
    public class DynamicAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        public bool Authorize { get; set; } = true;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userContext = context.HttpContext.RequestServices.GetService(typeof(IUserContext)) as IUserContext;
            var permissionService = context.HttpContext.RequestServices.GetService(typeof(IPermissionService)) as IPermissionService;

            if (userContext == null || permissionService == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!userContext.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var method = context.HttpContext.Request.Method;
            var rawPath = context.HttpContext.Request.Path.Value?.ToLower() ?? "";

            string path = rawPath.StartsWith("/api")
                ? rawPath.Substring(4)
                : rawPath;

            bool allowed = await permissionService.UserHasPermissionAsync(path, method);

            if (!allowed)
            {
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }
    }
}
