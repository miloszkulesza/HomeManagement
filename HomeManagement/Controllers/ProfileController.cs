using System.Security.Claims;
using AutoMapper;
using HomeManagement.Application.DTO;
using HomeManagement.Application.ViewModels;
using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public sealed class ProfileController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IAdminService _adminService;

    public ProfileController(IMapper mapper, IAdminService adminService)
    {
        _mapper = mapper;
        _adminService = adminService;
    }

    [HttpGet("me")]
    [ProducesResponseType(typeof(ApplicationUserVM), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApplicationUserVM>> GetCurrentUser()
    {
        var user = await _adminService.GetUserById(GetCurrentUserId())
            ?? throw new KeyNotFoundException("Nie znaleziono zalogowanego użytkownika.");

        return Ok(_mapper.Map<ApplicationUserVM>(user));
    }

    [HttpGet("users")]
    [ProducesResponseType(typeof(List<ApplicationUserVM>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ApplicationUserVM>>> GetHouseholdUsers()
    {
        var users = await _adminService.GetUsers();
        return Ok(_mapper.Map<List<ApplicationUserVM>>(users));
    }

    [HttpPut("me")]
    [ProducesResponseType(typeof(ApplicationUserVM), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApplicationUserVM>> UpdateCurrentUser(ApplicationUserUpdateDTO dto)
    {
        var domainUser = _mapper.Map<User>(dto);
        var updated = await _adminService.UpdatePutUserProfile(GetCurrentUserId(), domainUser);
        return Ok(_mapper.Map<ApplicationUserVM>(updated));
    }

    private string GetCurrentUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("Brak identyfikatora użytkownika w tokenie.");
}
