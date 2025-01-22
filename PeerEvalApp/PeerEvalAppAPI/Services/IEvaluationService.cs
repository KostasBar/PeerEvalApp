using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;

namespace PeerEvalAppAPI.Services
{
    public interface IEvaluationService
    {
        Task<Evaluation> AddEvaluation(SubmitEvaluationDTO submitEvaluationDTO);
        Task<List<EvalByGroupDTO>?> GetEvaluationByGroup(int groupID, int cycleId);
    }
}
