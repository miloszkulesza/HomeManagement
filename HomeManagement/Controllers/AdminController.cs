using AutoMapper;
using HomeManagement.Application.DTO;
using HomeManagement.Application.ViewModels;
using HomeManagement.Core.Consts;
using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HomeManagement.Controllers
{
    /// <summary>
    /// Operacje administracyjne dotyczące użytkowników i ról.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAdminService _adminService;

        /// <summary>
        /// Tworzy instancję <see cref="AdminController"/>.
        /// </summary>
        /// <param name="mapper">AutoMapper do mapowania DTO/VM.</param>
        /// <param name="adminService">Serwis aplikacyjny administracji.</param>
        public AdminController(IMapper mapper, IAdminService adminService)
        {
            _mapper = mapper;
            _adminService = adminService;
        }

        /// <summary>
        /// Pobierz listę wszystkich użytkowników.
        /// </summary>
        /// <returns>Lista użytkowników w postaci <see cref="ApplicationUserVM"/>.</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("Users")]
        [SwaggerOperation(Summary = "Pobierz użytkowników", Description = "Zwraca listę użytkowników wraz z rolami.")]
        [ProducesResponseType(typeof(List<ApplicationUserVM>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ApplicationUserVM>>> GetUsers()
        {
            var users = await _adminService.GetUsers();
            var vms = _mapper.Map<List<ApplicationUserVM>>(users);
            return Ok(vms);
        }

        /// <summary>
        /// Pobierz użytkownika po adresie email.
        /// </summary>
        /// <param name="email">Adres email użytkownika do wyszukania.</param>
        /// <returns>Pojedynczy użytkownik jako <see cref="ApplicationUserVM"/> lub 404 jeśli nie istnieje.</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("Users/{email}")]
        [SwaggerOperation(Summary = "Pobierz użytkownika", Description = "Zwraca użytkownika wraz z listą ról.")]
        [ProducesResponseType(typeof(ApplicationUserVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApplicationUserVM?>> GetUser(string email)
        {
            var user = await _adminService.GetUser(email);
            if (user is null) return NotFound();
            var vm = _mapper.Map<ApplicationUserVM>(user);
            return Ok(vm);
        }

        /// <summary>
        /// Aktualizuje profil użytkownika (PUT).
        /// </summary>
        /// <param name="id">Id użytkownika do aktualizacji.</param>
        /// <param name="dto">Dane aktualizacji użytkownika.</param>
        /// <returns>Zaktualizowany użytkownik jako <see cref="ApplicationUserVM"/>.</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("Users/{id}")]
        [SwaggerOperation(Summary = "Aktualizuj profil użytkownika", Description = "Zastępuje profil użytkownika wskazanego identyfikatorem.")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ApplicationUserVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApplicationUserVM?>> UpdatePutUserProfile(string id, ApplicationUserUpdateDTO dto)
        {
            var domainUser = _mapper.Map<User>(dto);
            var updated = await _adminService.UpdatePutUserProfile(id, domainUser);
            var vm = _mapper.Map<ApplicationUserVM>(updated);
            return Ok(vm);
        }

        /// <summary>
        /// Pobierz listę ról.
        /// </summary>
        /// <returns>Lista ról w postaci <see cref="IdentityRoleVM"/>.</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("Roles")]
        [SwaggerOperation(Summary = "Pobierz role", Description = "Zwraca listę ról z systemu.")]
        [ProducesResponseType(typeof(List<IdentityRoleVM>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<IdentityRoleVM>>> GetRoles()
        {
            var roles = await _adminService.GetRoles();
            var vms = _mapper.Map<List<IdentityRoleVM>>(roles);
            return Ok(vms);
        }
    }
}
