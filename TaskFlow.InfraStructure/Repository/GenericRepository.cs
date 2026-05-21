using Microsoft.EntityFrameworkCore;
using TaskFlow.InfraStructure.Data.DbContexts;
using TaskFlowDomain.Entites;
using TaskFlowDomain.Repository.Contract;

namespace TaskFlow.InfraStructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public Task<T> Update(T entity)
        {
            _dbSet.Update(entity);
            return Task.FromResult(entity);
        }

        public Task<int> Delete(T entity)
        {
            _dbSet.Remove(entity);
            return Task.FromResult(1);
        }
    }
}
