namespace PeerEvalAppAPI.Repositories
{
    public interface IUnitOfWork
    {
        UserRepository UserRepository { get; }
        EvaluationCycleRepository EvaluationCycleRepository { get; }

        Task<bool> SaveAsync();
    }
}
