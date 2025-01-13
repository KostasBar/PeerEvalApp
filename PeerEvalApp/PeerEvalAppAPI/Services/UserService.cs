using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using PeerEvalAppAPI.Core.Enums;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO.UserDTOs;
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

        public async Task<List<Evaluation>?> GetEvaluationsForUserAsync(int id)
        {
            List<Evaluation>? evalForUser = new();
            try
            {
                User? user = await _unitOfWork.UserRepository.GetUserById(id);
                if (user == null)
                {
                    throw new EntityNotFoundException("User", $"User with id {id} could not be found!");
                }
                evalForUser = await _unitOfWork.UserRepository.GetAllEvaluationsForUserAsync(user);
            }
            catch(EntityNotFoundException e) 
            {
                _logger.LogInformation($"{e.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetEvaluationsForUserAsync for id {id}", id);
                throw;
            }

            return evalForUser;
        }

        public async Task<List<User>?> GetUsersToEvaluateAsync(int id)
        {
            try
            {
                List<User>? usersToEvaluate = await _unitOfWork.UserRepository.GetUsersToEvaluate(id);
                return usersToEvaluate;
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogInformation(e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
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
