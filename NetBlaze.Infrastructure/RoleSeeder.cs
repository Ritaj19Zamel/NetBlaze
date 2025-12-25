using Microsoft.AspNetCore.Identity;
using NetBlaze.Domain.Entities.Identity;
namespace NetBlaze.Infrastructure
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<Role> roleManager)
        {
            string[] roleNames =
            {
                "Admin",
                "Manager",
                "Employee",
                "HR"
            };

            foreach (var roleName in roleNames)
            {
                var exists = await roleManager.RoleExistsAsync(roleName);
                if (exists)
                    continue;

                var role = Role.Create(roleName);

                // Audit fields (بما إن setters private)
                typeof(Role).GetProperty(nameof(Role.CreatedAt))!
                    .SetValue(role, DateTimeOffset.UtcNow);

                typeof(Role).GetProperty(nameof(Role.CreatedBy))!
                    .SetValue(role, "Seeder");

                typeof(Role).GetProperty(nameof(Role.IsActive))!
                    .SetValue(role, true);

                await roleManager.CreateAsync(role);
            }
        }
    }
}
