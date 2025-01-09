using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;

namespace PeerEvalAppAPI.Repositories
{
    public interface IEvaluationCycleRepository
    {
        Task<bool> OpenCycleExists();
        Task<EvaluationCycle?> UpdateEvaluationCycleAsync(EvaluationCycle evaluationCycle);
    }
}
