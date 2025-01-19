using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO.EvaluationCycleDTOs;

namespace PeerEvalAppAPI.Services
{
    public interface IEvaluationCycleService
    {
        Task<EvaluationCycle?> GetEvaluationCycleAsync(int id);
        Task InitializeEvaluationCycle(InitiateCycleDTO initiateCycleDTO);
        Task<EvaluationCycle?> UpdateEvaluateCycleAsync(UpdateCycleDTO updateCycleDTO);
        Task<EvaluationCycle?> EvaluationCycleExists();
    }
}
