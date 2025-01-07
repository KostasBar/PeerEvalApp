using PeerEvalAppAPI.Data;

namespace PeerEvalAppAPI.Repositories
{
    public interface IEvaluationCycleRepository
    {
        Task<bool> OpenCycleExists();
    }
}
