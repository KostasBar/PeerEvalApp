namespace PeerEvalAppAPI.Repositories
{
    public interface IUnitOfWork
    {
        UserRepository UserRepository { get; }
        EvaluationCycleRepository EvaluationCycleRepository { get; }
        EvaluationRepository EvaluationRepository { get; }
        EvaluationAnswerRepository EvaluationAnswerRepository { get; }
        QuestionRepository QuestionRepository { get; }
        GroupRepository GroupRepository { get; }
        Task<bool> SaveAsync();
    }
}
