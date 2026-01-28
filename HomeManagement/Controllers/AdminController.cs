using AutoMapper;
using HomeManagement.Application.DTO;
using HomeManagement.Application.ViewModels;
using HomeManagement.Core.Consts;
using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController(IMapper _mapper,
        IAdminService _adminService) : ControllerBase
    {
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("Users")]
        public async Task<ActionResult<List<ApplicationUserVM>>> GetUsers()
        {
            var users = await _adminService.GetUsers();
            var vms = _mapper.Map<List<ApplicationUserVM>>(users);
            return Ok(vms);
        }

        [Authorize(Roles = Roles.User)]
        [HttpGet]
        [Route("Users/{email}")]
        public async Task<ActionResult<ApplicationUserVM?>> GetUser(string email)
        {
            var user = await _adminService.GetUser(email);
            if (user is null) return NotFound();
            var vm = _mapper.Map<ApplicationUserVM>(user);
            return Ok(vm);
        }

        [Authorize(Roles = Roles.User)]
        [HttpPut]
        [Route("Users/{id}")]
        public async Task<ActionResult<ApplicationUserVM?>> UpdatePutUserProfile(string id, ApplicationUserUpdateDTO dto)
        {
            try
            {
                var domainUser = _mapper.Map<User>(dto);
                var updated = await _adminService.UpdatePutUserProfile(id, domainUser);
                var vm = _mapper.Map<ApplicationUserVM>(updated);
                return Ok(vm);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("Roles")]
        public async Task<ActionResult<List<IdentityRoleVM>>> GetRoles()
        {
            var roles = await _adminService.GetRoles();
            var vms = _mapper.Map<List<IdentityRoleVM>>(roles);
            return Ok(vms);
        }
    }
}
