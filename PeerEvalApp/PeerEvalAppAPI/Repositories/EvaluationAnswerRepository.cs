using PeerEvalAppAPI.Data;

namespace PeerEvalAppAPI.Repositories
{
    public class EvaluationAnswerRepository : BaseRepository<EvaluationAnswer>
    {
        public EvaluationAnswerRepository(PeerEvalAppDbContext dbContext) : base(dbContext)
        {
        }

    }
}
