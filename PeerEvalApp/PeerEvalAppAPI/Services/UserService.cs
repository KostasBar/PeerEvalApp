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
                var user = MapToUser(signUpDTO);

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

        public async Task<List<PastEvaluationsOfUserDTO>?> GetEvaluationsForUserAsync(int id)
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
                List<PastEvaluationsOfUserDTO> pastEvaluationsOfUserDTOs = await MapToPastEvaluationsOfUserDTO(evalForUser);
                return pastEvaluationsOfUserDTOs;
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
        }

        public async Task<List<UsersToEvaluateDTO>?> GetUsersToEvaluateAsync(int id)
        {
            try
            {
                List<User>? usersToEvaluate = await _unitOfWork.UserRepository.GetUsersToEvaluate(id);
                List<UsersToEvaluateDTO>? users = MapToUsersToEvaluateDTO(usersToEvaluate);
                return users;
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

        public async Task<List<UsersToEvaluateDTO>?> GetUsersByGroup(int groupId)
        {
            try
            {
                List<User>? users = await _unitOfWork.UserRepository.GetUsersByGroup(groupId);
                if (users is null)
                {
                    _logger.LogWarning("No users found for group id "+ groupId);
                    return new List<UsersToEvaluateDTO>();
                }
                List<UsersToEvaluateDTO> usersToEvaluateDTOs = MapToUsersToEvaluateDTO(users);
                return usersToEvaluateDTOs;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public string CreateUserToken(int userId, string email, string firstname, string lastname, UserRole? userRole,
            string appSecurityKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSecurityKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsInfo = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, firstname),
                new Claim(ClaimTypes.GivenName, lastname),
                new Claim(ClaimTypes.Role, userRole.ToString()!)
            };

            var jwtSecurityToken = new JwtSecurityToken("https://codingfactory.aueb.gr", "https://api.codingfactory.aueb.gr", claimsInfo, DateTime.UtcNow,
                DateTime.UtcNow.AddHours(3), signingCredentials);

            // Serialize the token
            var userToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return userToken;
        }

        /// <summary>
        /// Transforms a UserSignUpDTO to a User Object
        /// </summary>
        /// <param name="signUpDTO">The UserSignUpDTO to extract a UserFrom</param>
        /// <returns>A User</returns>
        private User MapToUser(UserSignUpDTO signUpDTO)
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

        /// <summary>
        /// Transform a list of Evaluations to a List of PastEvaluationsOfUserDTO
        /// </summary>
        /// <param name="evaluations">A list of evaluations</param>
        /// <returns>A List of PastEvaluationsOfUserDTO</returns>
        private async Task<List<PastEvaluationsOfUserDTO>> MapToPastEvaluationsOfUserDTO(List<Evaluation> evaluations)
        {
            List<PastEvaluationsOfUserDTO> pastEvaluationsOfUserDTOs = new List<PastEvaluationsOfUserDTO>();

            foreach (var item in evaluations)
            {
                float avg = 0;
                List<EvaluationAnswer>? answers = await _unitOfWork.EvaluationAnswerRepository.GetEvaluationAnswersOfEvaluation(item.Id);
                if (answers == null || !answers.Any())
                {
                    throw new EntityNotFoundException("EvaluationAnswer", "Error while trying to retrieve the evaluation answers for evaluation with id"+ item.Id +"!");
                }
                else
                {
                    avg = answers.Select(answer => float.Parse(answer.AnswerValue))
                                .DefaultIfEmpty(0) 
                                .Average();
                }
                EvaluationCycle? currentCycle = await _unitOfWork.EvaluationCycleRepository.GetAsync(item.EvaluationCycleId);
                Group? currentGroup = await _unitOfWork.GroupRepository.GetAsync(item.EvaluateeUser.GroupId);
                if (currentCycle != null && currentGroup != null)
                {
                    PastEvaluationsOfUserDTO pastEvaluationsDTO = new PastEvaluationsOfUserDTO()
                    {
                        CycleId = currentCycle.Id,
                        Department = item.EvaluateeUser.Group.GroupName,
                        CycleStartDate = currentCycle.StartDate,
                        CycleEndDate = currentCycle.EndDate,
                        AvgEvaluationResult = (float)Math.Round(avg, 2)
                    };
                    pastEvaluationsOfUserDTOs.Add(pastEvaluationsDTO);
                }
                else if(currentGroup == null)
                {
                    throw new EntityNotFoundException("Group", "Error while trying to identify the group of the evaluatee!");
                }
                else if(currentCycle == null)
                {
                    throw new EntityNotFoundException("Cycle", "Error while trying to retrieve the evaluation cycle of the evaluation!");
                }
            }
            return pastEvaluationsOfUserDTOs;
        }

        private List<UsersToEvaluateDTO> MapToUsersToEvaluateDTO(List<User> users)
        {
            List<UsersToEvaluateDTO> usersToEvaluateDTOs = new List<UsersToEvaluateDTO>();
            foreach (var user in users)
            {
                UsersToEvaluateDTO usersToEvaluateDTO = new UsersToEvaluateDTO()
                {
                    Id = user.Id,
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    Email = user.Email
                };

                usersToEvaluateDTOs.Add(usersToEvaluateDTO);
            }

            return usersToEvaluateDTOs;
        }

    }
}
