using Microsoft.EntityFrameworkCore;
using PeerEvalAppAPI.Data;

namespace PeerEvalAppAPI.Repositories
{
    public class EvaluationCycleRepository : BaseRepository<EvaluationCycle>, IEvaluationCycleRepository
    {
        public EvaluationCycleRepository(PeerEvalAppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> OpenCycleExists() => await _dbContext.EvaluationsCycles.AnyAsync(ec => ec.Status == 1);
    }
}
