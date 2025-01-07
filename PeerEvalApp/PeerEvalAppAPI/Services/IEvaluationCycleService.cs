using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;

namespace PeerEvalAppAPI.Services
{
    public interface IEvaluationCycleService
    {
        Task<EvaluationCycle?> GetEvaluationCycleAsync(int id);
        Task InitializeEvaluationCycle(InitiateCycleDTO initiateCycleDTO);
    }
}
