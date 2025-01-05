
using Microsoft.EntityFrameworkCore;
using PeerEvalAppAPI.Data;
using System.Collections.Generic;

namespace PeerEvalAppAPI.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly PeerEvalAppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(PeerEvalAppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>(); // Create dbset dynamically
        }

        public virtual async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public virtual async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);

        public virtual async Task<bool> DeleteAsync(int id)
        {
            T? existingEntity = await GetAsync(id);
            if (existingEntity is null) return false;
            _dbSet.Remove(existingEntity);
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public virtual async  Task<T?> GetAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<int> GetCountAsync() => await _dbSet.CountAsync();

        public Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
