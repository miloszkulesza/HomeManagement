using AutoMapper;
using HomeManagement.Core.DTO;
using HomeManagement.Core.Interfaces.Services;
using HomeManagement.Core.ViewModels;
using HomeManagement.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;

namespace HomeManagement.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AdminService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<ApplicationUserVM?> GetUser(string email)
        {
            ApplicationUser? user = _userManager.Users.FirstOrDefault(x => x.Email == email);
            if (user is null)
                return null;
            var userRoles = await _userManager.GetRolesAsync(user);
            var userVM = _mapper.Map<ApplicationUserVM>((user, userRoles));
            return userVM;
        }

        public async Task<ApplicationUserVM?> GetUserById(string id)
        {
            ApplicationUser? user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (user is null)
                return null;
            var userRoles = await _userManager.GetRolesAsync(user);
            var userVM = _mapper.Map<ApplicationUserVM>((user, userRoles));
            return userVM;
        }

        public async Task<List<ApplicationUserVM>> GetUsers()
        {
            List<ApplicationUser> users = _userManager.Users.ToList();
            List<ApplicationUserVM> userVMs = new List<ApplicationUserVM>();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userVMs.Add(_mapper.Map<ApplicationUserVM>((user, userRoles)));
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
                rolesVM.Add(_mapper.Map<IdentityRoleVM>((role, usersInRole)));
            }
            return rolesVM;
        }

        public async Task<ApplicationUserVM> UpdatePutUserProfile(string id, ApplicationUserUpdateDTO dto)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (user is null)
                throw new Exception($"Nie znaleziono użytkownika o identyfikatorze {id}");
            _mapper.Map(dto, user);
            await _userManager.UpdateAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            return _mapper.Map<ApplicationUserVM>((user, userRoles));
        }
    }
}
