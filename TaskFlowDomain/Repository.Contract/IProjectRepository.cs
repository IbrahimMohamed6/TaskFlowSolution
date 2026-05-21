using System;
using System.Collections.Generic;
using System.Text;
using TaskFlowDomain.Entites;

namespace TaskFlowDomain.Repository.Contract
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<bool> ExistsAsync(Guid id, string userId);
        Task<IReadOnlyList<Project>> GetByUserIdWithDetails(string userId);
        Task<Project?> GetByIdWithDetails(Guid id, string userId);
    }
}
