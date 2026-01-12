using Hangfire.Dashboard;

namespace NetBlaze.Api.Filters
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow all authenticated users for now, or specific roles if needed.
            // Example: return httpContext.User.Identity?.IsAuthenticated == true && httpContext.User.IsInRole("Admin");
            
            return httpContext.User.Identity?.IsAuthenticated == true;
        }
    }
}
