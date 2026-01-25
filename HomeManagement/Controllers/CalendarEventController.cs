using HomeManagement.Application.DTO;
using HomeManagement.Application.ViewModels;
using HomeManagement.Core.Consts;
using HomeManagement.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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
            try
            {
                var calendarEvents = await _calendarEventService.GetCalendarEvents();
                return Ok(calendarEvents);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CalendarEventVM>> CreateCalendarEvent(CalendarEventCreateDTO dto)
        {
            try
            {
                var createdEvent = await _calendarEventService.CreateCalendarEvent(new Core.Entities.CalendarEvent() { Title = "", UserId = ""});
                return Ok(createdEvent);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
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

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<CalendarEventVM>> UpdatePutCalendarEvent(string id, CalendarEventUpdateDTO dto)
        {
            try
            {
                var updateResult = await _calendarEventService
                    .UpdatePutCalendarEvent(new Core.Entities.CalendarEvent() { Title = "", UserId = ""});
                return Ok(updateResult);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }
    }
}
