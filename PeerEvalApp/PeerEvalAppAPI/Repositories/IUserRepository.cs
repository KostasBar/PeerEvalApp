using PeerEvalAppAPI.Data;

namespace PeerEvalAppAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync (string email, string password);
        Task<User?> UpdateUserAsync(int id, User user);
        Task<User?> GetByUsernameAsync(string email);
        Task<User?> GetUserById(int id);
        Task<List<User>> GetAllUsersFilteredPaginatedAsync(int pageNumber, int pageSize,
            List<Func<User, bool>> predicates);
        Task<List<Evaluation>?> GetAllEvaluationsForUserAsync(User user);
        Task<List<User>?> GetUsersToEvaluate(int id);
        Task<List<User>?> GetUsersByGroup(int groupId);
    }
}
