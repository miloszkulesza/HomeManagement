using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Services;

namespace HomeManagement.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public AdminService(IIdentityService identityService, IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        public Task<User?> GetUser(string email) => _identityService.GetUserByEmail(email);

        public Task<User?> GetUserById(string id) => _identityService.GetUserById(id);

        public Task<List<User>> GetUsers() => _identityService.GetUsers();

        public Task<List<Role>> GetRoles() => _identityService.GetRoles();

        public async Task<User> UpdatePutUserProfile(string id, User user)
        {
            await _identityService.UpdateUserAsync(id, user);
            var updated = await _identityService.GetUserById(id);
            if (updated is null) throw new Exception($"Nie znaleziono u¿ytkownika o identyfikatorze {id}");
            return updated;
        }
    }
}