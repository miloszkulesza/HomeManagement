using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HomeManagement.Core.Consts;

namespace HomeManagement.Infrastructure.Database;

public static class InitializeDatabase
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<HomeManagementContext>();
        await dbContext.Database.MigrateAsync();

        var config = serviceProvider.GetRequiredService<IConfiguration>();
        var initDbData = Convert.ToBoolean(config["InitDBSettings:InitDatabaseData"]);

        if (!initDbData)
            return;

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { Roles.Admin, Roles.User })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Could not create role '{role}': " +
                        string.Join(", ", result.Errors.Select(x => x.Description)));
                }
            }
        }

        var email = config["InitDBSettings:Email"];
        var password = config["InitDBSettings:Password"];

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Missing initialize database data with parameter InitDatabaseData set to true.");
        }

        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new ApplicationUser
            {
                Email = email,
                UserName = email,
                CalendarEventBackgroundColor = "#87cefa"
            };

            var createResult = await userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Could not create initial user: " +
                    string.Join(", ", createResult.Errors.Select(x => x.Description)));
            }
        }

        var roles = new[] { Roles.Admin, Roles.User };

        var rolesToAdd = new List<string>();

        foreach (var role in roles)
        {
            if (!await userManager.IsInRoleAsync(user, role))
                rolesToAdd.Add(role);
        }

        if (rolesToAdd.Count > 0)
        {
            var roleResult = await userManager.AddToRolesAsync(user, rolesToAdd);

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Could not assign initial user roles: " +
                    string.Join(", ", roleResult.Errors.Select(x => x.Description)));
            }
        }
    }
}