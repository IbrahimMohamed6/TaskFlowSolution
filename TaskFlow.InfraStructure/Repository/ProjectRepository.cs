using Microsoft.EntityFrameworkCore;
using TaskFlow.InfraStructure.Data.DbContexts;
using TaskFlowDomain.Entites;
using TaskFlowDomain.Repository.Contract;

namespace TaskFlow.InfraStructure.Repository
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<bool> ExistsAsync(Guid id, string userId)
        {
            return await _dbSet.AsNoTracking().AnyAsync(p => p.Id == id && p.UserId == userId);
        }

        public async Task<IReadOnlyList<Project>> GetByUserIdWithDetails(string userId)
        {
            return await _dbSet.AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.Tasks)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Project?> GetByIdWithDetails(Guid id, string userId)
        {
            return await _dbSet.AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
        }
    }
}
