using Microsoft.EntityFrameworkCore;
using PeerEvalAppAPI.Data;

namespace PeerEvalAppAPI.Repositories
{
    public class EvaluationAnswerRepository : BaseRepository<EvaluationAnswer>, IEvaluationAnswersRepository
    {
        public EvaluationAnswerRepository(PeerEvalAppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<EvaluationAnswer>?> GetEvaluationAnswersOfEvaluation(int evaluationId)
        {
            List<EvaluationAnswer> answers = await _dbContext.EvaluationsAnswers.Where(ea => ea.EvaluationId == evaluationId).ToListAsync();
            return answers;
        }

    }
}
