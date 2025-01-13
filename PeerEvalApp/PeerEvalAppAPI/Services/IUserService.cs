using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO.UserDTOs;

namespace PeerEvalAppAPI.Services
{
    public interface IUserService
    {
        Task<User?> VerifyAndGetUserAsync(UserLogInDTO credentials);
        Task<List<Evaluation>?> GetEvaluationsForUserAsync(int id);
        Task SignUpUserAsync(UserSignUpDTO signUpDTO);
        Task<List<User>?> GetUsersToEvaluateAsync(int id);
    }
}
