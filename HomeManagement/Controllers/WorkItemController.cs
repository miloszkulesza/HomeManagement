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
    /// Zarządzanie zadaniami (WorkItems).
    /// </summary>
    [Authorize(Roles = Roles.User)]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class WorkItemController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWorkItemService _workItemService;

        /// <summary>
        /// Tworzy instancję <see cref="WorkItemController"/>.
        /// </summary>
        /// <param name="mapper">AutoMapper do mapowania DTO/VM.</param>
        /// <param name="workItemService">Serwis aplikacyjny zarządzania zadaniami.</param>
        public WorkItemController(IMapper mapper, IWorkItemService workItemService)
        {
            _mapper = mapper;
            _workItemService = workItemService;
        }

        /// <summary>
        /// Pobierz wszystkie zadania.
        /// </summary>
        /// <returns>Lista zadań jako <see cref="WorkItemVM"/>.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Pobierz zadania", Description = "Zwraca listę wszystkich zadań.")]
        [ProducesResponseType(typeof(List<WorkItemVM>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<WorkItemVM>>> GetWorkItems()
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

        /// <summary>
        /// Utwórz zadanie.
        /// </summary>
        /// <param name="dto">Dane tworzonego zadania.</param>
        /// <returns>Utworzone zadanie jako <see cref="WorkItemVM"/>.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Utwórz zadanie", Description = "Tworzy nowe zadanie na podstawie WorkItemDto.")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(WorkItemVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Usuń zadanie po id.
        /// </summary>
        /// <param name="id">Id zadania do usunięcia (GUID jako string).</param>
        /// <returns>200 OK jeśli usunięto, 404 jeśli nie znaleziono.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Usuń zadanie", Description = "Usuwa zadanie o podanym id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Zastępuje zadanie (PUT).
        /// </summary>
        /// <param name="id">Id zadania do aktualizacji (GUID jako string).</param>
        /// <param name="dto">Dane do aktualizacji zadania.</param>
        /// <returns>Zaktualizowane zadanie jako <see cref="WorkItemVM"/>.</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Aktualizuj zadanie", Description = "Zastępuje zadanie o podanym id danymi z DTO.")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(WorkItemVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Usuń wszystkie ukończone zadania.
        /// </summary>
        /// <returns>200 OK po wykonaniu operacji.</returns>
        [HttpDelete("done")]
        [SwaggerOperation(Summary = "Usuń ukończone zadania", Description = "Usuwa wszystkie zadania oznaczone jako IsDone.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
