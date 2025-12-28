using HomeManagement.Core.Consts;
using HomeManagement.Core.DTO;
using HomeManagement.Core.Interfaces.Services;
using HomeManagement.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.Controllers
{
    [Authorize(Roles = Roles.User)]
    [ApiController]
    [Route("[controller]")]
    public class CalendarEventController : ControllerBase
    {
        private readonly ICalendarEventService _calendarEventService;

        public CalendarEventController(ICalendarEventService calendarEventService)
        {
            _calendarEventService = calendarEventService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CalendarEventVM>>> GetCalendarEvents()
        {
            var calendarEvents = await _calendarEventService.GetCalendarEvents();
            return Ok(calendarEvents);
        }

        [HttpPost]
        public async Task<ActionResult<CalendarEventVM>> CreateCalendarEvent(CalendarEventCreateDTO dto)
        {
            try
            {
                var createdEvent = await _calendarEventService.CreateCalendarEvent(dto);
                return Ok(createdEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
