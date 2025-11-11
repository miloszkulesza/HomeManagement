using HomeManagement.Core.Interfaces;
using HomeManagement.Core.ViewModels;
using HomeManagement.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;

namespace HomeManagement.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ApplicationUserVM?> GetUser(string email)
        {
            ApplicationUser? user = _userManager.Users.FirstOrDefault(x => x.Email == email);

            if (user is null)
                return null;

            var userRoles = await _userManager.GetRolesAsync(user);
            ApplicationUserVM userVM = new ApplicationUserVM
            {
                Email = email,
                Roles = userRoles.ToList()
            };
            return userVM;
        }

        public async Task<List<ApplicationUserVM>> GetUsers()
        {
            List<ApplicationUser> users = _userManager.Users.ToList();
            List<ApplicationUserVM> userVMs = new List<ApplicationUserVM>();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userVMs.Add(new ApplicationUserVM
                {
                    Email = user.Email,
                    Roles = userRoles.ToList()
                });
            }
            return userVMs;
        }

        public async Task<List<IdentityRoleVM>> GetRoles()
        {
            var roles = _roleManager.Roles.ToList();
            var rolesVM = new List<IdentityRoleVM>();
            foreach (var role in roles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name ?? role.Id);
                rolesVM.Add(new IdentityRoleVM
                {
                    Id = role.Id,
                    Name = role.Name,
                    NormalizedName = role.NormalizedName,
                    Users = usersInRole.Select(x => x.Email!).ToList()
                });
            }
            return rolesVM;
        }
    }
}
