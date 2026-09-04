using System.Security.Claims;
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
    /// Zarządzanie wydarzeniami kalendarza.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class CalendarEventController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICalendarEventService _calendarEventService;
        private readonly IAdminService _adminService;

        /// <summary>
        /// Tworzy instancję <see cref="CalendarEventController"/>.
        /// </summary>
        /// <param name="mapper">AutoMapper do konwersji DTO/VM.</param>
        /// <param name="calendarEventService">Serwis aplikacyjny wydarzeń kalendarza.</param>
        /// <param name="adminService">Serwis dostarczający dane autorów wydarzeń.</param>
        public CalendarEventController(
            IMapper mapper,
            ICalendarEventService calendarEventService,
            IAdminService adminService)
        {
            _mapper = mapper;
            _calendarEventService = calendarEventService;
            _adminService = adminService;
        }

        /// <summary>
        /// Pobierz wszystkie wydarzenia kalendarza.
        /// </summary>
        /// <returns>Lista wydarzeń jako <see cref="CalendarEventVM"/>.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Pobierz wydarzenia", Description = "Zwraca wszystkie wydarzenia kalendarza.")]
        [ProducesResponseType(typeof(List<CalendarEventVM>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<CalendarEventVM>>> GetCalendarEvents()
        {
            var calendarEvents = await _calendarEventService.GetCalendarEvents();
            var usersById = (await _adminService.GetUsers()).ToDictionary(user => user.Id);
            var vms = calendarEvents.Select(calendarEvent =>
            {
                if (!usersById.TryGetValue(calendarEvent.UserId, out var user))
                    throw new KeyNotFoundException($"Nie znaleziono autora wydarzenia {calendarEvent.Id}.");

                return _mapper.Map<CalendarEventVM>((calendarEvent, user));
            }).ToList();

            return Ok(vms);
        }

        /// <summary>
        /// Utwórz nowe wydarzenie kalendarza.
        /// </summary>
        /// <param name="dto">Dane tworzonego wydarzenia.</param>
        /// <returns>Utworzone wydarzenie jako <see cref="CalendarEventVM"/>.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Utwórz wydarzenie", Description = "Tworzy nowe wydarzenie kalendarza.")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CalendarEventVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CalendarEventVM>> CreateCalendarEvent([FromBody] CalendarEventCreateDTO dto)
        {
            var domain = _mapper.Map<CalendarEvent>(dto);
            domain.UserId = GetCurrentUserId();
            var created = await _calendarEventService.CreateCalendarEvent(domain);
            var vm = await MapViewModel(created);
            return CreatedAtAction(nameof(GetCalendarEvents), new { id = created.Id }, vm);
        }

        /// <summary>
        /// Usuń wydarzenie kalendarza po identyfikatorze.
        /// </summary>
        /// <param name="id">Id wydarzenia do usunięcia (GUID jako string).</param>
        /// <returns>200 OK jeśli usunięto, 404 jeśli nie znaleziono.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Usuń wydarzenie", Description = "Usuwa wydarzenie kalendarza o podanym id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCalendarEvent(Guid id)
        {
            await _calendarEventService.RemoveCalendarEvent(id);
            return NoContent();
        }

        /// <summary>
        /// Zastępuje wydarzenie kalendarza (PUT).
        /// </summary>
        /// <param name="id">Id wydarzenia do zastąpienia (GUID jako string).</param>
        /// <param name="dto">Dane wydarzenia użyte do zastąpienia.</param>
        /// <returns>Zaktualizowane wydarzenie jako <see cref="CalendarEventVM"/>.</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Aktualizuj wydarzenie", Description = "Zastępuje wydarzenie kalendarza o danym id.")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CalendarEventVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CalendarEventVM>> UpdatePutCalendarEvent(Guid id, [FromBody] CalendarEventUpdateDTO dto)
        {
            var domain = _mapper.Map<CalendarEvent>(dto);
            var updated = await _calendarEventService.UpdatePutCalendarEvent(id, domain);
            var vm = await MapViewModel(updated);
            return Ok(vm);
        }

        private string GetCurrentUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Brak identyfikatora użytkownika w tokenie.");

        private async Task<CalendarEventVM> MapViewModel(CalendarEvent calendarEvent)
        {
            var user = await _adminService.GetUserById(calendarEvent.UserId)
                ?? throw new KeyNotFoundException($"Nie znaleziono autora wydarzenia {calendarEvent.Id}.");

            return _mapper.Map<CalendarEventVM>((calendarEvent, user));
        }
    }
}
