using Microsoft.EntityFrameworkCore;
using PeerEvalAppAPI.Data;

namespace PeerEvalAppAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PeerEvalAppDbContext _dbContext;

        public UnitOfWork(PeerEvalAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserRepository UserRepository =>new UserRepository(_dbContext);
        public EvaluationCycleRepository EvaluationCycleRepository => new EvaluationCycleRepository(_dbContext);

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
