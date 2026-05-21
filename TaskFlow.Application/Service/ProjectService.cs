using TaskFlow.Application.Common;
using TaskFlowDomain.DTOs.ProjectDTOs;
using TaskFlowDomain.DTOs.TaskItemsDTOs;
using TaskFlowDomain.Entites;
using TaskFlowDomain.Repository.Contract;
using TaskFlowDomain.Service.Contract;

namespace TaskFlow.Application.Service
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProjectToReturnDto>> GetAllProjects(string userId)
        {
            var projects = await _unitOfWork.Projects.GetByUserIdWithDetails(userId);

            return projects.Select(p => new ProjectToReturnDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                UserId = p.UserId,
                UserName = p.User?.FullName ?? string.Empty,
                Tasks = p.Tasks?.Select(t => t.Title).ToList() ?? new List<string>()
            });
        }

        public async Task<ProjectToReturnDto?> GetProjectById(Guid id, string userId)
        {
            var project = await _unitOfWork.Projects.GetByIdWithDetails(id, userId);
            if (project is null)
            {
                return null;
            }

            return new ProjectToReturnDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                UserId = project.UserId,
                UserName = project.User?.FullName ?? string.Empty,
                Tasks = project.Tasks?.Select(t => t.Title).ToList() ?? new List<string>()
            };
        }

        public async Task<ProjectToReturnDto> AddProject(CreateProjectDto createProject, string userId)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(createProject.Name))
            {
                errors.Add("Name is required.");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }

            var project = new Project
            {
                Name = createProject.Name.Trim(),
                Description = createProject.Description?.Trim() ?? string.Empty,
                UserId = userId
            };

            await _unitOfWork.Projects.Add(project);
            await _unitOfWork.SaveChangesAsync();

            var projectDetails = await _unitOfWork.Projects.GetByIdWithDetails(project.Id, project.UserId);
            var createdProject = projectDetails ?? project;

            return new ProjectToReturnDto
            {
                Id = createdProject.Id,
                Name = createdProject.Name,
                Description = createdProject.Description,
                CreatedAt = createdProject.CreatedAt,
                UserId = createdProject.UserId,
                UserName = createdProject.User?.FullName ?? string.Empty,
                Tasks = createdProject.Tasks?.Select(t => t.Title).ToList() ?? new List<string>()
            };
        }

        public async Task<ProjectToReturnDto> UpdateProject(UpdateProjectDto updateProject, string userId)
        {
            var errors = new List<string>();

            if (updateProject.Id == Guid.Empty)
            {
                errors.Add("Id is required.");
            }

            if (string.IsNullOrWhiteSpace(updateProject.Name))
            {
                errors.Add("Name is required.");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }

            var project = await _unitOfWork.Projects.GetById(updateProject.Id);
            if (project is null || project.UserId != userId)
            {
                throw new NotFoundException(nameof(Project), updateProject.Id);
            }

            project.Name = updateProject.Name.Trim();
            project.Description = updateProject.Description?.Trim() ?? string.Empty;
            project.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Projects.Update(project);
            await _unitOfWork.SaveChangesAsync();

            var projectDetails = await _unitOfWork.Projects.GetByIdWithDetails(project.Id, userId);
            var updatedProject = projectDetails ?? project;

            return new ProjectToReturnDto
            {
                Id = updatedProject.Id,
                Name = updatedProject.Name,
                Description = updatedProject.Description,
                CreatedAt = updatedProject.CreatedAt,
                UserId = updatedProject.UserId,
                UserName = updatedProject.User?.FullName ?? string.Empty,
                Tasks = updatedProject.Tasks?.Select(t => t.Title).ToList() ?? new List<string>()
            };
        }

        public async Task<bool> DeleteProject(Guid id, string userId)
        {
            if (id == Guid.Empty)
            {
                throw new ValidationException(new List<string> { "Id is required." });
            }

            var project = await _unitOfWork.Projects.GetById(id);
            if (project is null || project.UserId != userId)
            {
                return false;
            }

            await _unitOfWork.Projects.Delete(project);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
