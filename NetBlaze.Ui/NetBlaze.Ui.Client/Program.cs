using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Extensions;
using NetBlaze.Ui.Client.InternalHelperTypes.General;
using NetBlaze.Ui.Client.Services.CommonServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var urlConfiguration = builder.Configuration.GetSection(nameof(UrlConfiguration)).Get<UrlConfiguration>()!;

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.RegisterClientServices(urlConfiguration);


builder.Services.AddAuthorizationCore(options =>
{
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

var host = builder.Build();

await host.ConsumeClientServicesAsync();

await host.RunAsync();
