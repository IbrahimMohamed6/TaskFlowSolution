using System;
using System.Collections.Generic;
using System.Text;
using TaskFlowDomain.DTOs.ProjectDTOs;
using TaskFlowDomain.DTOs.TaskItemsDTOs;

namespace TaskFlowDomain.Service.Contract
{
    public interface ITaskItemsService
    {
        Task<IEnumerable<TaskItemsToReturnDto>> GetAllTaskItems(string userId);

        Task<TaskItemsToReturnDto?> GetTaskItemById(Guid id, string userId);

        Task<IEnumerable<TaskItemsToReturnDto>> GetTasksByProject(Guid projectId, string userId);

        Task<TaskItemsToReturnDto> AddTaskItem(CreateTaskItemDto createTaskItem, string userId);

        Task<TaskItemsToReturnDto> UpdateTaskItem(UpdateTaskItemDto updateTaskItem, string userId);

        Task<TaskItemsToReturnDto> UpdateTaskStatus(Guid taskItemId, UpdateTaskStatuesDto updateTaskStatus, string userId);

        Task<bool> DeleteTaskItem(Guid id, string userId);
    }
}
