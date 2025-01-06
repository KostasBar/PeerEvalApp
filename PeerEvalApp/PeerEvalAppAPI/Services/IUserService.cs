using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;

namespace PeerEvalAppAPI.Services
{
    public interface IUserService
    {
        Task<User?> VerifyAndGetUserAsync(UserLogInDTO credentials);
        Task SignUpUserAsync(UserSignUpDTO signUpDTO);
    }
}
