using PeerEvalAppAPI.Data;

namespace PeerEvalAppAPI.Repositories
{
    public interface IEvaluationAnswersRepository
    {
        Task<List<EvaluationAnswer>?> GetEvaluationAnswersOfEvaluation(int id);
    }
}
