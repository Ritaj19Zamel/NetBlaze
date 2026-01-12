using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Domain.DatabaseObjects.CommonInterfaces;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.SharedKernel.Enums;
using System;
using System.Reflection;


namespace NetBlaze.Infrastructure.Data.DatabaseContext
{
    public static class InitializerExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

            await initializer.InitializeAsync();

            await ApplyMigrationsViewsSPsIfNotAsync(app);

            await initializer.SeedAsync();
        }
        private static async Task ApplyMigrationsViewsSPsIfNotAsync(WebApplication webApplication)
        {
            using var scope = webApplication?.Services.CreateScope();

            var dbContext = scope?.ServiceProvider.GetService<ApplicationDbContext>();

            if (dbContext is not null)
            {
                // WARNING: Missing with methods order can lead to errors when applying database objects.

                await dbContext.Database.MigrateAsync();

                await ApplyViewsInDatabaseAsync(dbContext);

            }
        }
        private static async Task ApplyViewsInDatabaseAsync(ApplicationDbContext dbContext)
        {
            var coreAssembly = Assembly.GetAssembly(typeof(IDatabaseView));

            var availableViewsTypes = coreAssembly?
                .GetTypes()
                .Where(t => typeof(IDatabaseView).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in availableViewsTypes ?? [])
            {
                if (Activator.CreateInstance(type) is IDatabaseView view)
                {
                    await dbContext.Database.ExecuteSqlRawAsync(view.CreateOrReplaceCommand);
                }
            }
        }

    }

    public class ApplicationDbContextInitializer
    {
        private readonly ILogger<ApplicationDbContextInitializer> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;


        public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger,
                                               ApplicationDbContext context,
                                               IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _context = context;
            _unitOfWork = unitOfWork;
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

            //await TrySeedSystemPredefinedRolesAsync();
            //await TrySeedDepartmentsAsync();
            //await TrySeedPoliciesAsync();
            //await TrySeedRootAccountAsync();
            //await TrySeedUserRolesAsync();
            //await TrySeedVacationsAsync();
        }

        private async Task TrySeedSystemPredefinedRolesAsync()
        {
            if (_context.Roles.Any())
                return;

            var roles = new[]
            {
                Role.Create("SuperAdmin"),
                Role.Create("Admin"),
                Role.Create("Manager"),
                Role.Create("Employee")
            };

            _context.Roles.AddRange(roles);
            await _context.SaveChangesAsync();
        }
        private async Task TrySeedDepartmentsAsync()
        {
            if (_context.Departments.Any())
                return;

            var departments = new[]
            {
                new Department { DepartmentName = "HR" },
                new Department { DepartmentName = "IT" },
                new Department { DepartmentName = "Finance" },
                new Department { DepartmentName = "Operations" }
            };

            _context.Departments.AddRange(departments);
            await _context.SaveChangesAsync();
        }
        private async Task TrySeedPoliciesAsync()
        {
            if (_context.Policies.Any())
                return;

            var policies = new[]
            {
                new Policy
                {
                    PolicyName = "Late Check In",
                    PolicyCode = "CHECKIN",
                    WorkStartTime = new TimeOnly(9, 0),
                    WorkEndTime = new TimeOnly(9, 30),
                    PolicyType = PolicyType.penality,
                    ActionValue = 1
                }
            };

            _context.Policies.AddRange(policies);
            await _context.SaveChangesAsync();
        }
        private async Task TrySeedRootAccountAsync()
        {
            if (_context.Users.Any())
                return;

            var hrDepartment = _context.Departments.First();

            var user = new User
            {
                UserName = "root",
                NormalizedUserName = "ROOT",
                Email = "root@gmail.com",
                NormalizedEmail = "ROOT@GEMAIL.COM",
                EmailConfirmed = true,
                DisplayName = "System Administrator",
                DepartmentId = hrDepartment.Id,
                PhoneNumber = "01000000000",
                PhoneNumberConfirmed = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        private async Task TrySeedUserRolesAsync()
        {
            if (_context.UserRoles.Any())
                return;

            var user = _context.Users.First();
            var role = _context.Roles.First(r => r.Name == "SuperAdmin");

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
        }
        private async Task TrySeedVacationsAsync()
        {
            if (_context.Vacations.Any())
                return;

            var vacations = new[]
            {
                new Vacation { DayName = DayOfWeek.Friday, IsVacation = true, IsRecurring = true },
                new Vacation { DayName = DayOfWeek.Saturday, IsVacation = true, IsRecurring = true },
                new Vacation { DayDate = new DateOnly(2025, 1, 7), IsVacation = true },
                new Vacation { DayDate = new DateOnly(2025, 4, 25), IsVacation = true }
            };

            _context.Vacations.AddRange(vacations);
            await _context.SaveChangesAsync();
        }



    }
}