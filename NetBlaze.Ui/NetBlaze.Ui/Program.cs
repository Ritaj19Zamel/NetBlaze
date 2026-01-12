using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure authentication but allow anonymous for Blazor WebAssembly routes
// Authentication is handled client-side via JWT tokens
builder.Services
    .AddAuthentication("UI")
    .AddCookie("UI", options =>
    {
        options.LoginPath = "/";
        options.AccessDeniedPath = "/";
        // Allow anonymous access - authentication is handled client-side
        options.Events.OnRedirectToLogin = context =>
        {
            // Don't block - let Blazor WebAssembly handle authentication
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            // Don't block - let Blazor WebAssembly handle authorization
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization(options =>
{
    // Allow anonymous by default - authentication is handled client-side via JWT
    options.FallbackPolicy = null;
    
    options.AddPolicy(AuthorizationPolicies.SuperAdmin, policy =>
        policy.RequireRole(AppRoles.SuperAdmin.ToString()));

    options.AddPolicy(AuthorizationPolicies.HRAndManager, policy =>
        policy.RequireRole(
            AppRoles.HR.ToString(),
            AppRoles.Manager.ToString()));

    options.AddPolicy(AuthorizationPolicies.Employee, policy =>
        policy.RequireRole(AppRoles.Employee.ToString()));

    options.AddPolicy(AuthorizationPolicies.AnyAuthenticated, policy =>
        policy.RequireAuthenticatedUser());
});

builder.Services.AddCascadingAuthenticationState();

builder.RegisterServerServices();

builder.Services.AddServerSideBlazor(options =>
{
    options.DetailedErrors = true;
});


var app = builder.Build();

// Authentication is handled client-side via JWT tokens
// Only use server-side auth if needed for specific server-side routes
// For Blazor WebAssembly, authentication is handled entirely client-side
app.UseAuthentication();
app.UseAuthorization();

app.ConsumeServerServices();

await app.RunAsync();
