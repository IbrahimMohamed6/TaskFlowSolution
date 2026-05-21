using TaskFlowDomain.Entites;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TaskFlowDomain.Repository.Contract
{
    public interface ITaskItemRepository : IGenericRepository<TaskItem>
    {
         Task UpdateTaskStatusAsync(Guid taskItemId, TaskFlowDomain.Entites.TaskStatus status, CancellationToken cancellationToken = default);
            


        Task<IReadOnlyList<TaskItem>> GetTasksByProject(Guid projectId, CancellationToken cancellationToken = default);

    }
}
