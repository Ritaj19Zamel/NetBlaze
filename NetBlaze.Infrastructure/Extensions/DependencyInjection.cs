using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.Infrastructure.Data.DatabaseContext;
using NetBlaze.Infrastructure.Data.GenericRepository;
using NetBlaze.Infrastructure.Data.Interceptors;
using NetBlaze.Infrastructure.Data.ParallelService;
using NetBlaze.Infrastructure.Data.UnitOfWork;
using NetBlaze.Infrastructure.GenericMemoryCacheRepository;
using NetBlaze.Infrastructure.InfraServices;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString(MiscConstants.Default);

            builder.Services.AddScoped<ISaveChangesInterceptor, BeforeSaveChangesInterceptor>();

          

            builder.Services.AddDbContextFactory<ApplicationDbContext>((sp, opt) =>
            {
                opt.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                opt.EnableThreadSafetyChecks(true);
            }, ServiceLifetime.Scoped);


            builder.Services.AddDbContextFactory<ApplicationDbContext>((sp, opt) =>
            {
                opt.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
           
                opt.EnableThreadSafetyChecks(true);
            }, ServiceLifetime.Scoped);

            builder.Services.AddScoped<ApplicationDbContextInitializer>();

            builder.Services.AddHttpClient();
            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders(); 

            builder.Services.AddSingleton(TimeProvider.System);

            builder.Services.AddScoped<IRepository, Repository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IJwtBearerService, JwtBearerService>();

            builder.Services.AddScoped<IParallelQueryService, ParallelQueryService>();

            builder.Services.AddMemoryCache();

            builder.Services.AddSingleton<IMemoryCacheRepository, MemoryCacheRepository>();

            builder.AddBearerAuthenticationService();

             builder.Services.AddAuthorization(options =>
            {
                // Set fallback policy to prevent challenge errors
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
        }
    }
}