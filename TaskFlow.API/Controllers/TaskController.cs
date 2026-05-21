using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Application.Common;
using TaskFlowDomain.DTOs.TaskItemsDTOs;
using TaskFlowDomain.Service.Contract;

namespace TaskFlow.API.Controllers
{
   
    public class TaskController : BaseApiController
    { 
        private readonly ITaskItemsService _taskItemsService;

        public TaskController(ITaskItemsService taskItemsService)
        {
            _taskItemsService = taskItemsService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TaskItemsToReturnDto>>>> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<IEnumerable<TaskItemsToReturnDto>>.Fail("User is not authenticated properly."));
            }

            var taskItems = await _taskItemsService.GetAllTaskItems(userId);
            return Ok(ApiResponse<IEnumerable<TaskItemsToReturnDto>>.Ok(taskItems));
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<TaskItemsToReturnDto>>> GetById(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<TaskItemsToReturnDto>.Fail("User is not authenticated properly."));
            }

            var taskItem = await _taskItemsService.GetTaskItemById(id, userId);
            if (taskItem is null)
            {
                return NotFound(ApiResponse<TaskItemsToReturnDto>.Fail($"TaskItem with id ({id}) was not found."));
            }

            return Ok(ApiResponse<TaskItemsToReturnDto>.Ok(taskItem));
        }

        [Authorize]
        [HttpGet("project/{projectId:guid}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TaskItemsToReturnDto>>>> GetByProject(Guid projectId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<IEnumerable<TaskItemsToReturnDto>>.Fail("User is not authenticated properly."));
            }

            var taskItems = await _taskItemsService.GetTasksByProject(projectId, userId);
            return Ok(ApiResponse<IEnumerable<TaskItemsToReturnDto>>.Ok(taskItems));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<TaskItemsToReturnDto>>> Create([FromBody] CreateTaskItemDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<TaskItemsToReturnDto>.Fail("User is not authenticated properly."));
            }

            var created = await _taskItemsService.AddTaskItem(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ApiResponse<TaskItemsToReturnDto>.Ok(created, "Created"));
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<TaskItemsToReturnDto>>> Update(Guid id, [FromBody] UpdateTaskItemDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<TaskItemsToReturnDto>.Fail("User is not authenticated properly."));
            }

            if (dto.Id == Guid.Empty)
            {
                dto.Id = id;
            }

            if (dto.Id != id)
            {
                return BadRequest(ApiResponse<TaskItemsToReturnDto>.Fail("Route id does not match body id."));
            }

            var updated = await _taskItemsService.UpdateTaskItem(dto, userId);
            return Ok(ApiResponse<TaskItemsToReturnDto>.Ok(updated, "Updated"));
        }

        [Authorize]
        [HttpPut("{id:guid}/status")]
        public async Task<ActionResult<ApiResponse<TaskItemsToReturnDto>>> UpdateStatus(Guid id, [FromBody] UpdateTaskStatuesDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<TaskItemsToReturnDto>.Fail("User is not authenticated properly."));
            }

            var updated = await _taskItemsService.UpdateTaskStatus(id, dto, userId);
            return Ok(ApiResponse<TaskItemsToReturnDto>.Ok(updated, "Updated"));
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<bool>.Fail("User is not authenticated properly."));
            }

            var deleted = await _taskItemsService.DeleteTaskItem(id, userId);
            if (!deleted)
            {
                return NotFound(ApiResponse<bool>.Fail($"TaskItem with id ({id}) was not found."));
            }

            return Ok(ApiResponse<bool>.Ok(true, "Deleted"));
        }
    }
}
