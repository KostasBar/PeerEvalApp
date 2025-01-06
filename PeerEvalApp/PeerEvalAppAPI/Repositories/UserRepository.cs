using Microsoft.EntityFrameworkCore;
using PeerEvalAppAPI.Data;
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
    }
}
