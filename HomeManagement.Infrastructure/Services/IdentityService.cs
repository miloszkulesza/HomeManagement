using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeManagement.Core.Entities;
using HomeManagement.Core.Exceptions;
using HomeManagement.Core.Interfaces.Services;
using HomeManagement.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            var result = _mapper.Map<User>(user);
            result.Roles = roles.ToList();

            return result;
        }

        public async Task<User?> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            var result = _mapper.Map<User>(user);
            result.Roles = roles.ToList();

            return result;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<User>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var mappedUser = _mapper.Map<User>(user);
                mappedUser.Roles = roles.ToList();

                result.Add(mappedUser);
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
            var foundUser = await _userManager.FindByIdAsync(id);
            if (foundUser is null)
                throw new KeyNotFoundException($"Nie znaleziono użytkownika o identyfikatorze {id}.");

            foundUser.CalendarEventBackgroundColor = user.CalendarEventBackgroundColor;
            var result = await _userManager.UpdateAsync(foundUser);
            if (!result.Succeeded)
                throw new ConflictException(string.Join(", ", result.Errors.Select(x => x.Description)));
        }
    }
}
