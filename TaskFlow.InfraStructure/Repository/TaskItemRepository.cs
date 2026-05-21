using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaskFlow.InfraStructure.Data.DbContexts;
using TaskFlowDomain.Entites;
using TaskFlowDomain.Repository.Contract;

namespace TaskFlow.InfraStructure.Repository
{
    public class TaskItemRepository : GenericRepository<TaskItem>, ITaskItemRepository
    {
        public TaskItemRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<IReadOnlyList<TaskItem>> GetTasksByProject(Guid projectId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking()
                .Where(t => t.ProjectId == projectId)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateTaskStatusAsync(Guid taskItemId, TaskFlowDomain.Entites.TaskStatus status, CancellationToken cancellationToken = default)
        {
            var taskItem = await _dbSet.FirstOrDefaultAsync(t => t.Id == taskItemId, cancellationToken);

            if (taskItem is null)
            {
                return;
            }

            taskItem.Status = status;
            taskItem.UpdatedAt = DateTime.UtcNow;
        }
    }
}
