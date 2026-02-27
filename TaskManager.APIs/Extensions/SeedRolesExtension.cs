using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities.Identity;
using TaskManager.Repository.Data.Context;

namespace TaskManager.APIs.Extensions
{
    public static class SeedRolesExtension
    {
        public static async Task SeedRolesAndMigrateAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("SeedRolesExtension");

            try
            {
                // Step 1 - Run pending migrations automatically
                var dbContext = services.GetRequiredService<TaskManagerDbContext>();
                await dbContext.Database.MigrateAsync();
                logger.LogInformation("Database migrated successfully");

                // Step 2 - Seed roles if they don't exist
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                var roles = new[] { "Admin", "TeamLeader", "TeamMember" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        var result = await roleManager.CreateAsync(new IdentityRole(role));

                        if (!result.Succeeded)
                            foreach (var error in result.Errors)
                                logger.LogError("Failed to create role {Role}: {Error}", role, error.Description);
                        else
                            logger.LogInformation("Role created: {Role}", role);
                    }
                    else
                        logger.LogInformation("Role already exists: {Role}", role);
                }

                // Step 3 - Seed default admin user
                // Why seed admin? -> Register endpoint is Admin-only
                //                    app needs one admin to exist from
                //                    the start to create all other users
                //                    this solves the chicken-and-egg problem
                var adminEmail = "admin@taskmanager.com";
                var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

                if (existingAdmin is null)
                {
                    var admin = new ApplicationUser
                    {
                        DisplayName = "System Admin",
                        Email = adminEmail,
                        UserName = adminEmail
                    };

                    var result = await userManager.CreateAsync(admin, "Admin@123456");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                        logger.LogInformation("Default admin seeded: {Email}", adminEmail);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                            logger.LogError("Failed to seed admin: {Error}", error.Description);
                    }
                }
                else
                {
                    logger.LogInformation("Admin already exists: {Email}", adminEmail);
                }

                logger.LogInformation("Seeding completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during migration or seeding");
            }
        }
    }
}
