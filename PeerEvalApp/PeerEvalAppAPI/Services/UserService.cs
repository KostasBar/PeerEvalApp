using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using PeerEvalAppAPI.Core.Enums;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;
using PeerEvalAppAPI.Exceptions;
using PeerEvalAppAPI.Repositories;
using PeerEvalAppAPI.Security;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PeerEvalAppAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = new LoggerFactory().AddSerilog().CreateLogger<UserService>();
        }

        public async Task<User?> VerifyAndGetUserAsync(UserLogInDTO credentials)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserAsync(credentials.Email, credentials.Password);

                if (user == null)
                {
                    _logger.LogInformation("No user found for email {Email}. Returning null.", credentials.Email);
                    return null;
                }

                _logger.LogInformation("User with Email: {Email} found and returned!", user.Email);

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while verifying user.");
                throw;
            }
        }

        public async Task SignUpUserAsync(UserSignUpDTO signUpDTO)
        {
            try
            {
                var user = ExtractUser(signUpDTO);

                User? existingUser = await _unitOfWork.UserRepository.GetByUsernameAsync(user.Email);
                if (existingUser != null)
                {
                    throw new EntityAlreadyExistsException(
                        "User",
                        $"User with email '{existingUser.Email}' already exists."
                    );
                }

                user.Password = EncryptionUtil.Encrypt(user.Password);

                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("User with email: {Email} signed up successfully.", user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during SignUpUserAsync for email {Email}", signUpDTO.Email);
                throw;
            }
        }

        public string CreateUserToken(int userId, string email, UserRole? userRole,
            string appSecurityKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSecurityKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsInfo = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, userRole.ToString()!)
            };

            var jwtSecurityToken = new JwtSecurityToken("https://codingfactory.aueb.gr", "https://api.codingfactory.aueb.gr", claimsInfo, DateTime.UtcNow,
                DateTime.UtcNow.AddHours(3), signingCredentials);

            // Serialize the token
            var userToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return userToken;
        }

        private User ExtractUser(UserSignUpDTO signUpDTO)
        {
            return new User()
            {
                FirstName = signUpDTO.FirstName,
                LastName = signUpDTO.LastName,
                Email = signUpDTO.Email,
                Password = signUpDTO.Password,
                Role = signUpDTO.UserRole,
                Manager = null,
                Group = null,
                GroupId = signUpDTO.GroupId,
                EvaluationsMade = new List<Evaluation>(),
                EvaluationsReceived = new List<Evaluation>(),
                Subordinates = new List<User>()
            };
        }


    }
}
