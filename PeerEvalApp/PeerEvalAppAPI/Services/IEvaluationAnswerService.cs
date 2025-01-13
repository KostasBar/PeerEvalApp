using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;

namespace PeerEvalAppAPI.Services
{
    public interface IEvaluationAnswerService
    {
        Task AddEvaluationAnswer(SubmitEvaluationDTO submitEvaluationDTO, Evaluation evaluation);
    }
}
