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
    [Authorize(Roles = Roles.User)]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class CalendarEventController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICalendarEventService _calendarEventService;

        /// <summary>
        /// Tworzy instancję <see cref="CalendarEventController"/>.
        /// </summary>
        /// <param name="mapper">AutoMapper do konwersji DTO/VM.</param>
        /// <param name="calendarEventService">Serwis aplikacyjny wydarzeń kalendarza.</param>
        public CalendarEventController(IMapper mapper, ICalendarEventService calendarEventService)
        {
            _mapper = mapper;
            _calendarEventService = calendarEventService;
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
            try
            {
                var calendarEvents = await _calendarEventService.GetCalendarEvents();
                var vms = _mapper.Map<List<CalendarEventVM>>(calendarEvents);
                return Ok(vms);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
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
            try
            {
                var domain = _mapper.Map<CalendarEvent>(dto);
                var created = await _calendarEventService.CreateCalendarEvent(domain);
                var vm = _mapper.Map<CalendarEventVM>(created);
                return Ok(vm);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
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
        public async Task<ActionResult> DeleteCalendarEvent(string id)
        {
            try
            {
                await _calendarEventService.RemoveCalendarEvent(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
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
        public async Task<ActionResult<CalendarEventVM>> UpdatePutCalendarEvent(string id, [FromBody] CalendarEventUpdateDTO dto)
        {
            try
            {
                var domain = _mapper.Map<CalendarEvent>(dto);
                domain.Id = Guid.TryParse(id, out var g) ? g : domain.Id;
                var updateResult = await _calendarEventService.UpdatePutCalendarEvent(domain);
                var vm = _mapper.Map<CalendarEventVM>(updateResult);
                return Ok(vm);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }
    }
}
