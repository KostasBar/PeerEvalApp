using Microsoft.EntityFrameworkCore;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.Exceptions;
using PeerEvalAppAPI.Security;

namespace PeerEvalAppAPI.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(PeerEvalAppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User?> GetByUsernameAsync(string email) 
            => await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email); 

        public async Task<User?> GetUserAsync(string email, string password)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            // Return null either if the email doesn't exist or if the password (of the returned user) is not valid
            if (user == null) return null;

            if (!EncryptionUtil.IsValidPassword(password, user.Password)) return null;

            return user;
        }

        public async Task<User?> GetUserById(int id)
            => await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            User? checkUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (checkUser == null) return null;
            if (checkUser.Id != id) return null; // Doublecheck that the given ID is correct!

            _dbContext.Users.Attach(checkUser);
            _dbContext.Entry(checkUser).State = EntityState.Modified;

            return checkUser;
        }

        public async Task<List<User>> GetAllUsersFilteredPaginatedAsync(int pageNumber, int pageSize, List<Func<User, bool>> predicates)
        {
            int skip = (pageNumber - 1) * pageSize;
            IQueryable<User> query = _dbContext.Users.Skip(skip).Take(pageSize);

            // Check if predicate exists else return everything
            if (predicates != null && predicates.Any())
            {
                query = query.Where(u => predicates.All(predicate => predicate(u)));
            }
            return await query.ToListAsync();
        }

        public async Task<List<Evaluation>?> GetAllEvaluationsForUserAsync(User user)
        {
            if (user == null)
            {
             return new List<Evaluation>();
            }

            var evalForUser = await _dbContext.Evaluations
                .Where(e => e.EvaluateeUserId == user.Id)
                .ToListAsync();

            return evalForUser;
        }

        public async Task<List<User>?> GetUsersToEvaluate(int id)
        {

            // Fetch the user to ensure it exists and to retrieve the GroupId for the filter
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new EntityNotFoundException("User", $"User with id {id} could not be found!");
            }

            // Fetch the current active evaluation cycle's ID with Status = 0
            int? activeEvaluationCycleId = await _dbContext.EvaluationsCycles
                .Where(c => c.Status == 0)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (activeEvaluationCycleId == null)
            {
                throw new EntityNotFoundException("EvaluationCycle","No active evaluation cycle found.");
            }

            // Get users to evaluate, excluding the current user and those already evaluated in the current cycle
            List<User>? usersToEvaluate = await _dbContext.Users
                .Where(u => u.GroupId == user.GroupId && u.Id != user.Id)
                .Where(u => !_dbContext.Evaluations
                    .Any(e => e.EvaluateeUserId == u.Id && e.EvaluationCycleId == activeEvaluationCycleId))
                .ToListAsync();

            return usersToEvaluate;
        }
    }
}
