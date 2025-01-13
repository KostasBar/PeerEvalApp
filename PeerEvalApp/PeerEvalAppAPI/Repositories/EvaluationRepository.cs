using Microsoft.EntityFrameworkCore;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;

namespace PeerEvalAppAPI.Repositories
{
    public class EvaluationRepository : BaseRepository<Evaluation>, IEvaluationRepository
    {
        public EvaluationRepository(PeerEvalAppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Evaluation?> GetEvaluationByIdAsync(int id)
            => await _dbContext.Evaluations.FirstOrDefaultAsync(e => e.Id == id);

        public async Task<List<Evaluation>? > GetEvaluationsByCycle(int cycleId)
        {
            List<Evaluation>? evaluations = await _dbContext.Evaluations
                .Where(e => e.EvaluationCycleId == cycleId).ToListAsync();
            return evaluations;
        }
    }
}
