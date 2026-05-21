using TaskFlow.InfraStructure.Data.DbContexts;
using TaskFlowDomain.Entites;
using TaskFlowDomain.Repository.Contract;

namespace TaskFlow.InfraStructure.Repository
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IProjectRepository? _projects;
        private ITaskItemRepository? _taskItems;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IProjectRepository Projects => _projects ??= new ProjectRepository(_context);

        public ITaskItemRepository TaskItems => _taskItems ??= new TaskItemRepository(_context);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => _context.SaveChangesAsync(cancellationToken);

        public ValueTask DisposeAsync()
            => _context.DisposeAsync();
    }
}
