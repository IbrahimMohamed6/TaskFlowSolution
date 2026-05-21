using TaskFlow.Application.Common;
using TaskFlowDomain.DTOs.TaskItemsDTOs;
using TaskFlowDomain.Entites;
using TaskFlowDomain.Repository.Contract;
using TaskFlowDomain.Service.Contract;

namespace TaskFlow.Application.Service
{
    public class TaskItemsService : ITaskItemsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskItemsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TaskItemsToReturnDto>> GetAllTaskItems(string userId)
        {
            var projects = await _unitOfWork.Projects.GetByUserIdWithDetails(userId);
            var taskItems = projects.SelectMany(p => p.Tasks ?? new List<TaskItem>()).ToList();

            return taskItems.Select(t => new TaskItemsToReturnDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate,
                Project = t.Project?.Name ?? string.Empty,
                CreatedAt = t.CreatedAt
            });
        }

        public async Task<TaskItemsToReturnDto?> GetTaskItemById(Guid id, string userId)
        {
            var taskItem = await _unitOfWork.TaskItems.GetById(id);
            if (taskItem is null)
            {
                return null;
            }

            var project = await _unitOfWork.Projects.GetById(taskItem.ProjectId);
            if (project is null || project.UserId != userId)
            {
                return null;
            }

            return new TaskItemsToReturnDto
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                Status = taskItem.Status,
                Priority = taskItem.Priority,
                DueDate = taskItem.DueDate,
                Project = project.Name,
                CreatedAt = taskItem.CreatedAt
            };
        }

        public async Task<IEnumerable<TaskItemsToReturnDto>> GetTasksByProject(Guid projectId, string userId)
        {
            if (projectId == Guid.Empty)
            {
                throw new ValidationException(["ProjectId is required."]);
            }

            var project = await _unitOfWork.Projects.GetByIdWithDetails(projectId, userId);
            if (project is null)
            {
                throw new NotFoundException(nameof(Project), projectId);
            }

            return project.Tasks?.Select(t => new TaskItemsToReturnDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate,
                Project = project.Name,
                CreatedAt = t.CreatedAt
            }) ?? new List<TaskItemsToReturnDto>();
        }

        public async Task<TaskItemsToReturnDto> AddTaskItem(CreateTaskItemDto createTaskItem, string userId)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(createTaskItem.Title))
            {
                errors.Add("Title is required.");
            }

            if (createTaskItem.ProjectId == Guid.Empty)
            {
                errors.Add("ProjectId is required.");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }

            var project = await _unitOfWork.Projects.GetById(createTaskItem.ProjectId);
            if (project is null || project.UserId != userId)
            {
                throw new NotFoundException(nameof(Project), createTaskItem.ProjectId);
            }

            var taskItem = new TaskItem
            {
                Title = createTaskItem.Title.Trim(),
                Description = createTaskItem.Description?.Trim() ?? string.Empty,
                Priority = createTaskItem.Priority,
                DueDate = createTaskItem.DueDate,
                ProjectId = createTaskItem.ProjectId,
                Status = TaskFlowDomain.Entites.TaskStatus.Todo
            };

            await _unitOfWork.TaskItems.Add(taskItem);
            await _unitOfWork.SaveChangesAsync();

            return new TaskItemsToReturnDto
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                Status = taskItem.Status,
                Priority = taskItem.Priority,
                DueDate = taskItem.DueDate,
                Project = project.Name,
                CreatedAt = taskItem.CreatedAt
            };
        }

        public async Task<TaskItemsToReturnDto> UpdateTaskItem(UpdateTaskItemDto updateTaskItem, string userId)
        {
            var errors = new List<string>();

            if (updateTaskItem.Id == Guid.Empty)
            {
                errors.Add("Id is required.");
            }

            if (string.IsNullOrWhiteSpace(updateTaskItem.Title))
            {
                errors.Add("Title is required.");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }

            var taskItem = await _unitOfWork.TaskItems.GetById(updateTaskItem.Id);
            if (taskItem is null)
            {
                throw new NotFoundException(nameof(TaskItem), updateTaskItem.Id);
            }

            var project = await _unitOfWork.Projects.GetById(taskItem.ProjectId);
            if (project is null || project.UserId != userId)
            {
                throw new NotFoundException(nameof(TaskItem), updateTaskItem.Id);
            }

            taskItem.Title = updateTaskItem.Title.Trim();
            taskItem.Description = updateTaskItem.Description?.Trim() ?? string.Empty;
            taskItem.Priority = updateTaskItem.Priority;
            taskItem.DueDate = updateTaskItem.DueDate;
            taskItem.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.TaskItems.Update(taskItem);
            await _unitOfWork.SaveChangesAsync();

            project = await _unitOfWork.Projects.GetById(taskItem.ProjectId);

            return new TaskItemsToReturnDto
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                Status = taskItem.Status,
                Priority = taskItem.Priority,
                DueDate = taskItem.DueDate,
                Project = project?.Name ?? string.Empty,
                CreatedAt = taskItem.CreatedAt
            };
        }

        public async Task<TaskItemsToReturnDto> UpdateTaskStatus(Guid taskItemId, UpdateTaskStatuesDto updateTaskStatus, string userId)
        {
            if (taskItemId == Guid.Empty)
            {
                throw new ValidationException(["Id is required."]);
            }

            var taskItem = await _unitOfWork.TaskItems.GetById(taskItemId);
            if (taskItem is null)
            {
                throw new NotFoundException(nameof(TaskItem), taskItemId);
            }

            var project = await _unitOfWork.Projects.GetById(taskItem.ProjectId);
            if (project is null || project.UserId != userId)
            {
                throw new NotFoundException(nameof(TaskItem), taskItemId);
            }

            taskItem.Status = updateTaskStatus.Status;
            taskItem.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.TaskItems.Update(taskItem);
            await _unitOfWork.SaveChangesAsync();

            return new TaskItemsToReturnDto
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                Status = taskItem.Status,
                Priority = taskItem.Priority,
                DueDate = taskItem.DueDate,
                Project = project.Name,
                CreatedAt = taskItem.CreatedAt
            };
        }

        public async Task<bool> DeleteTaskItem(Guid id, string userId)
        {
            if (id == Guid.Empty)
            {
                throw new ValidationException(["Id is required."]);
            }

            var taskItem = await _unitOfWork.TaskItems.GetById(id);
            if (taskItem is null)
            {
                return false;
            }

            var project = await _unitOfWork.Projects.GetById(taskItem.ProjectId);
            if (project is null || project.UserId != userId)
            {
                return false;
            }

            await _unitOfWork.TaskItems.Delete(taskItem);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
