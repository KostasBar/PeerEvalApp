using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;

namespace PeerEvalAppAPI.Services
{
    public interface IEvaluationService
    {
        Task<Evaluation> AddEvaluation(SubmitEvaluationDTO submitEvaluationDTO);
    }
}
