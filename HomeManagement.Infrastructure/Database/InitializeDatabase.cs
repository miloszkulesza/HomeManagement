using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using HomeManagement.Core.Consts;

namespace HomeManagement.Infrastructure.Database
{
    public static class InitializeDatabase
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<HomeManagementContext>();
            dbContext.Database.Migrate();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            if (!await roleManager.RoleExistsAsync(Roles.User))
                await roleManager.CreateAsync(new IdentityRole(Roles.User));
        }
    }
}
