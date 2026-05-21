using TaskFlowDomain.Entites;

namespace TaskFlowDomain.Repository.Contract
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IProjectRepository Projects { get; }
        ITaskItemRepository TaskItems { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
