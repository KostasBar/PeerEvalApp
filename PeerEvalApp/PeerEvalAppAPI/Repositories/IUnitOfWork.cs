namespace PeerEvalAppAPI.Repositories
{
    public interface IUnitOfWork
    {
        UserRepository UserRepository { get; }
        EvaluationCycleRepository EvaluationCycleRepository { get; }
        EvaluationRepository EvaluationRepository { get; }
        EvaluationAnswerRepository EvaluationAnswerRepository { get; }
        QuestionRepository QuestionRepository { get; }
        Task<bool> SaveAsync();
    }
}
