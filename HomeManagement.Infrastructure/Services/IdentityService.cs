using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Services;
using HomeManagement.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;

namespace HomeManagement.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public IdentityService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Email == email);
            if (user is null) return null;
            var roles = await _userManager.GetRolesAsync(user);
            return _mapper.Map<User>((user, roles));
        }

        public async Task<User?> GetUserById(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (user is null) return null;
            var roles = await _userManager.GetRolesAsync(user);
            return _mapper.Map<User>((user, roles));
        }

        public async Task<List<User>> GetUsers()
        {
            var users = _userManager.Users.ToList();
            var result = new List<User>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                result.Add(_mapper.Map<User>((u, roles)));
            }
            return result;
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            if (user is null) return new List<string>();
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<List<Role>> GetRoles()
        {
            var identityRoles = _roleManager.Roles.ToList();
            return _mapper.Map<List<Role>>(identityRoles);
        }

        public async Task UpdateUserAsync(string id, User user)
        {
            var foundUser = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (foundUser is null) throw new Exception($"Nie znaleziono u¿ytkownika o identyfikatorze {id}");
            _mapper.Map(user, foundUser);
            await _userManager.UpdateAsync(foundUser);
        }
    }
}