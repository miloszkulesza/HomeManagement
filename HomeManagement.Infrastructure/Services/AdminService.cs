using AutoMapper;
using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Services;
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

        public async Task<User?> GetUser(string email)
        {
            ApplicationUser? user = _userManager.Users.FirstOrDefault(x => x.Email == email);
            if (user is null)
                return null;
            var userRoles = await _userManager.GetRolesAsync(user);
            var userVM = _mapper.Map<User>((user, userRoles));
            return userVM;
        }

        public async Task<User?> GetUserById(string id)
        {
            ApplicationUser? user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (user is null)
                return null;
            var userRoles = await _userManager.GetRolesAsync(user);
            var userVM = _mapper.Map<User>((user, userRoles));
            return userVM;
        }

        public async Task<List<User>> GetUsers()
        {
            List<ApplicationUser> users = _userManager.Users.ToList();
            List<User> userVMs = new List<User>();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userVMs.Add(_mapper.Map<User>((user, userRoles)));
            }
            return userVMs;
        }

        public async Task<List<Role>> GetRoles()
        {
            var identityRoles = _roleManager.Roles.ToList();
            var roles = _mapper.Map<List<Role>>(identityRoles);
            return roles;
        }

        public async Task<User> UpdatePutUserProfile(string id, User user)
        {
            var foundUser = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (foundUser is null)
                throw new Exception($"Nie znaleziono użytkownika o identyfikatorze {id}");
            _mapper.Map(user, foundUser);
            await _userManager.UpdateAsync(foundUser);
            var userRoles = await _userManager.GetRolesAsync(foundUser);
            return _mapper.Map<User>((user, userRoles));
        }
    }
}
