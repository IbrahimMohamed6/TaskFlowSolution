using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Application.Common;
using TaskFlowDomain.DTOs.ProjectDTOs;
using TaskFlowDomain.Service.Contract;

namespace TaskFlow.API.Controllers
{
    
    public class ProjectController : BaseApiController
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService; 
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProjectToReturnDto>>>> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<IEnumerable<ProjectToReturnDto>>.Fail("User is not authenticated properly."));
            }

            var projects = await _projectService.GetAllProjects(userId);
            return Ok(ApiResponse<IEnumerable<ProjectToReturnDto>>.Ok(projects));
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<ProjectToReturnDto>>> GetById(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<ProjectToReturnDto>.Fail("User is not authenticated properly."));
            }

            var project = await _projectService.GetProjectById(id, userId);
            if (project is null)
            {
                return NotFound(ApiResponse<ProjectToReturnDto>.Fail($"Project with id ({id}) was not found."));
            }

            return Ok(ApiResponse<ProjectToReturnDto>.Ok(project));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProjectToReturnDto>>> Create([FromBody] CreateProjectDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<ProjectToReturnDto>.Fail("User is not authenticated properly."));
            }

            var created = await _projectService.AddProject(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ApiResponse<ProjectToReturnDto>.Ok(created, "Created"));
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<ProjectToReturnDto>>> Update(Guid id, [FromBody] UpdateProjectDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<ProjectToReturnDto>.Fail("User is not authenticated properly."));
            }

            if (dto.Id == Guid.Empty)
            {
                dto.Id = id;
            }

            if (dto.Id != id)
            {
                return BadRequest(ApiResponse<ProjectToReturnDto>.Fail("Route id does not match body id."));
            }

            var updated = await _projectService.UpdateProject(dto, userId);
            return Ok(ApiResponse<ProjectToReturnDto>.Ok(updated, "Updated"));
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

            var deleted = await _projectService.DeleteProject(id, userId);
            if (!deleted)
            {
                return NotFound(ApiResponse<bool>.Fail($"Project with id ({id}) was not found."));
            }

            return Ok(ApiResponse<bool>.Ok(true, "Deleted"));
        }

    }
}
