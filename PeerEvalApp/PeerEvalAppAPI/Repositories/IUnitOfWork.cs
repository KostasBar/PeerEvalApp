namespace PeerEvalAppAPI.Repositories
{
    public interface IUnitOfWork
    {
        UserRepository UserRepository { get; }

        Task<bool> SaveAsync();
    }
}
