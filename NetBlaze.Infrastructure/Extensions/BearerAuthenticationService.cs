using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NetBlaze.SharedKernel.HelperUtilities.General;
using System.Security.Claims;
using System.Text;

namespace NetBlaze.Infrastructure.Extensions
{
    public static class BearerAuthenticationService
    {
        public static void AddBearerAuthenticationService(this IHostApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

                var key = Encoding.UTF8.GetBytes(jwtSettings!.Key);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    RoleClaimType = ClaimTypes.Role, 
                    NameClaimType = ClaimTypes.NameIdentifier
                };

                // Configure events to handle authentication failures gracefully
                options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        // Prevent automatic challenge and return 401 directly
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
