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
    [Authorize(Roles = Roles.User)]
    [ApiController]
    [Route("[controller]")]
    public class CalendarEventController(IMapper _mapper,
        ICalendarEventService _calendarEventService) : ControllerBase
    {
        [HttpGet]
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

        [HttpPost]
        public async Task<ActionResult<CalendarEventVM>> CreateCalendarEvent(CalendarEventCreateDTO dto)
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
