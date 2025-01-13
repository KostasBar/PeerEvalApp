using PeerEvalAppAPI.Data;

namespace PeerEvalAppAPI.Repositories
{
    public class QuestionRepository : BaseRepository<Question>
    {
        public QuestionRepository(PeerEvalAppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
