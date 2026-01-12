using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.Ui.Client.InternalHelperTypes.General;
using NetBlaze.Ui.Client.Services;
using NetBlaze.Ui.Client.Services.CommonServices;
using System.Globalization;

namespace NetBlaze.Ui.Client.Extensions
{
    public static class DIServicesEntryPoint
    {
        public static void RegisterClientServices(this IServiceCollection services, UrlConfiguration urlConfiguration)
        {
            services.AddBlazoredLocalStorage();

            services.AddTransient<AuthTokenHandler>();

            services.AddTransient<HttpRequestHandler>();

            services.AddScoped<AuthGuard>();


            services
                .AddHttpClient<ExternalHttpClientWrapper>(client => client.BaseAddress = new Uri(urlConfiguration.ApiBaseUrl))
                .AddHttpMessageHandler<HttpRequestHandler>()
                .AddHttpMessageHandler<AuthTokenHandler>();

            services
                .AddHttpClient<InternalHttpClientWrapper>(client => client.BaseAddress = new Uri(urlConfiguration.UiBaseUrl));

            services.AddLocalization();

            services.AddMudServices();

            services.AddScoped<CentralizedSnackbarProvider>();

            services.AddScoped<CookieService>();


            // ADD BLAZOR SERVICES HERE:

            services.AddScoped<BlazSampleService>();
            services.AddScoped<BlazeDepartmentService>();
            services.AddScoped<BlazeRoleService>();
            services.AddScoped<BlazeUserService>();
            services.AddScoped<BlazeAuthService>();
            services.AddScoped<BlazeFidoService>();
            services.AddScoped<BlazeVacationService>();
            services.AddScoped<BlazePolicyService>();
            services.AddScoped<BlazeAttendenceService>();
            services.AddScoped<BlazeRandomCheckService>();

            services.AddScoped<JwtAuthStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(sp =>
                sp.GetRequiredService<JwtAuthStateProvider>());
            services.AddScoped<IJwtAuthService>(sp =>
                sp.GetRequiredService<JwtAuthStateProvider>());

        }

        public static async Task ConsumeClientServicesAsync(this WebAssemblyHost app)
        {
            var cookieService = app.Services.GetRequiredService<CookieService>();

            var currentCulture = await cookieService.GetCookieAsync(MiscConstants.currentCultureCode);

            string languageCode = LanguageCode.ENGLISH_CODE;

            if (!string.IsNullOrWhiteSpace(currentCulture))
            {
                languageCode = currentCulture;
            }

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(languageCode);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(languageCode);

            await Task.CompletedTask;
        }
    }
}