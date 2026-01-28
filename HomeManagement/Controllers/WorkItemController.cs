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
    public class WorkItemController(IMapper _mapper, 
        IWorkItemService _workItemService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<WorkItem>>> GetWorkItems()
        {
            try
            {
                var items = await _workItemService.GetWorkItems();
                var vms = _mapper.Map<List<WorkItemVM>>(items);
                return Ok(vms);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<WorkItemVM>> CreateWorkItem([FromBody] WorkItemDto dto)
        {
            try
            {
                var entity = _mapper.Map<WorkItem>(dto);
                var created = await _workItemService.CreateWorkItem(entity);
                var vm = _mapper.Map<WorkItemVM>(created);
                return Ok(vm);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWorkItem(string id)
        {
            try
            {
                await _workItemService.RemoveWorkItem(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WorkItemVM>> UpdatePutWorkItem(string id, [FromBody] WorkItemDto dto)
        {
            try
            {
                if (!Guid.TryParse(id, out var guid))
                    return BadRequest("Nieprawidłowe id");

                var entity = _mapper.Map<WorkItem>(dto);
                entity.Id = guid;
                var updated = await _workItemService.UpdatePutWorkItem(entity);
                var vm = _mapper.Map<WorkItemVM>(updated);
                return Ok(vm);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpDelete("done")]
        public async Task<ActionResult> DeleteDoneWorkItems()
        {
            try
            {
                await _workItemService.DeleteDoneWorkItems();
                return Ok();
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }
    }
}
