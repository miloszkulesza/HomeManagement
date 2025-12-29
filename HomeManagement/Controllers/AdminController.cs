using HomeManagement.Core.Consts;
using HomeManagement.Core.DTO;
using HomeManagement.Core.Interfaces.Services;
using HomeManagement.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService) 
        {
            _adminService = adminService;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("Users")]
        public async Task<ActionResult<List<ApplicationUserVM>>> GetUsers()
        {
            var user = await _adminService.GetUsers();
            return Ok(user);
        }

        [Authorize(Roles = Roles.User)]
        [HttpGet]
        [Route("Users/{email}")]
        public async Task<ActionResult<ApplicationUserVM?>> GetUser(string email)
        {
            var user = await _adminService.GetUser(email);
            return Ok(user);
        }

        [Authorize(Roles = Roles.User)]
        [HttpPut]
        [Route("Users/{id}")]
        public async Task<ActionResult<ApplicationUserVM?>> UpdatePutUserProfile(string id, ApplicationUserUpdateDTO dto)
        {
            try
            {
                var user = await _adminService.UpdatePutUserProfile(id, dto);
                return Ok(user);
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
            return Ok(roles);
        }
    }
}
