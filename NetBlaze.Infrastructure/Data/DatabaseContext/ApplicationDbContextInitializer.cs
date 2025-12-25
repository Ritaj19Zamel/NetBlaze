using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Domain.Entities.Identity;

namespace NetBlaze.Infrastructure.Data.DatabaseContext
{
    public static class InitializerExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

            await initializer.InitializeAsync();

            await initializer.SeedAsync();
        }
    }

    public class ApplicationDbContextInitializer
    {
        private readonly ILogger<ApplicationDbContextInitializer> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<Role> _roleManager;


        public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger,
                                               ApplicationDbContext context,
                                               IUnitOfWork unitOfWork,
                                                RoleManager<Role> roleManager
                                               )
        {
            _logger = logger;
            _context = context;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            await _context.Database.MigrateAsync();
        }

        public async Task SeedAsync()
        {
            await TrySeedAsync();
        }

        public async Task TrySeedAsync()
        {
            // WARNING: Missing with methods order can lead to errors when seeding the database for the first time.

            await TrySeedSystemPredefinedRolesAsync();

            await TrySeedRootAccountAsync();
        }

        private async Task TrySeedSystemPredefinedRolesAsync()
        {
            string[] roles =
   {
        "Admin",
        "Manager",
        "Employee"
    };

            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var role = Role.Create(roleName);
                    await _roleManager.CreateAsync(role);
                }
            }   
        }
      


        private async Task TrySeedRootAccountAsync()
        {

        }
    }
}