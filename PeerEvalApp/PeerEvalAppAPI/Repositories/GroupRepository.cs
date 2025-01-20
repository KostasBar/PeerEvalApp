using PeerEvalAppAPI.Data;

namespace PeerEvalAppAPI.Repositories
{
    public class GroupRepository : BaseRepository<Group>
    {
        public GroupRepository(Data.PeerEvalAppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
