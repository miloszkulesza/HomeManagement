using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using HomeManagement.Core.Consts;
using Microsoft.Extensions.Configuration;

namespace HomeManagement.Infrastructure.Database
{
    public static class InitializeDatabase
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<HomeManagementContext>();
            dbContext.Database.Migrate();
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var initDbData = Convert.ToBoolean(config["InitDBSettings:InitDatabaseData"]);
            if (initDbData)
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManager.RoleExistsAsync(Roles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                if (!await roleManager.RoleExistsAsync(Roles.User))
                    await roleManager.CreateAsync(new IdentityRole(Roles.User));
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var email = config["InitDBSettings:Email"];
                var normalizedEmail = config["InitDBSettings:NormalizedEmail"];
                var password = config["InitDBSettings:Password"];
                if (string.IsNullOrEmpty(email) ||
                     string.IsNullOrEmpty(normalizedEmail) ||
                     string.IsNullOrEmpty(password))
                    throw new Exception("Missing initialize database data with parameter InitDatabaseData set to true");
                if (!userManager.Users.Any(x => x.Email == email))
                {
                    var user = new ApplicationUser()
                    {
                        Email = email,
                        NormalizedEmail = normalizedEmail,
                        UserName = email,
                        NormalizedUserName = normalizedEmail,
                        CalendarEventBackgroundColor = "#87cefa"
                    };
                    await userManager.CreateAsync(user, password!);
                    await userManager.AddToRolesAsync(user, new[] { Roles.Admin, Roles.User });
                }
            }
        }
    }
}
