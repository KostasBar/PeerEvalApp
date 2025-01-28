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

        /// <summary>
        /// Verifies the user's credentials (email and password) and returns the user if found.
        /// </summary>
        /// <param name="credentials">The login credentials of the user (email and password).</param>
        /// <returns>The user if the credentials are valid, otherwise null.</returns>
        /// <exception cref="Exception">Thrown if an error occurs while verifying the user's credentials.</exception>
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

        /// <summary>
        /// Registers a new user in the system after verifying that the user does not already exist.
        /// </summary>
        /// <param name="signUpDTO">The signup details of the new user.</param>
        /// <exception cref="EntityAlreadyExistsException">Thrown if a user with the same email already exists.</exception>
        /// <exception cref="Exception">Thrown if any other error occurs during the signup process.</exception>
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

        /// <summary>
        /// Updates the details of an existing user.
        /// </summary>
        /// <param name="userUpdateDTO">The details of the user to be updated.</param>
        /// <exception cref="EntityNotFoundException">Thrown if the user to update cannot be found.</exception>
        /// <exception cref="Exception">Thrown if any error occurs while updating the user.</exception>
        public async Task UpdateUserAsync(UserUpdateDTO userUpdateDTO)
        {
            try
            {

                User? existing = await _unitOfWork.UserRepository.GetAsync(userUpdateDTO.Id);

                if (existing is null)
                {
                    throw new EntityNotFoundException("User", "User with id " + userUpdateDTO.Id + " could not be retrieved!");
                }
                existing.FirstName = userUpdateDTO.Firstname;
                existing.LastName = userUpdateDTO.Lastname;
                existing.Email = userUpdateDTO.Email;
                existing.Password = !string.IsNullOrEmpty(userUpdateDTO.Password) ? EncryptionUtil.Encrypt(userUpdateDTO.Password) : existing.Password;
                existing.Role = Enum.TryParse<UserRole>(userUpdateDTO.Role, out UserRole roleResult) ? roleResult : existing.Role;
                existing.GroupId = userUpdateDTO.GroupId != 0 ? userUpdateDTO.GroupId : existing.GroupId;
                await _unitOfWork.SaveAsync();
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                throw;  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves past evaluations for a specific user based on their user ID.
        /// </summary>
        /// <param name="id">The user ID for whom the evaluations are to be retrieved.</param>
        /// <returns>A list of PastEvaluationsOfUserDTOs representing the past evaluations of the user.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the user cannot be found.</exception>
        /// <exception cref="Exception">Thrown if any error occurs while retrieving the evaluations for the user.</exception>
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
            catch (EntityNotFoundException e)
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

        /// <summary>
        /// Retrieves a list of users that a specific user can evaluate based on their user ID.
        /// </summary>
        /// <param name="id">The user ID to fetch the users that the specified user can evaluate.</param>
        /// <returns>A list of UsersToEvaluateDTOs representing the users to be evaluated.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if no users are found for evaluation.</exception>
        /// <exception cref="Exception">Thrown if any error occurs while retrieving the users to evaluate.</exception>
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

        /// <summary>
        /// Retrieves a list of users from a specific group based on the group ID.
        /// </summary>
        /// <param name="groupId">The ID of the group to fetch users from.</param>
        /// <returns>A list of UsersToEvaluateDTOs representing the users in the specified group.</returns>
        /// <exception cref="Exception">Thrown if any error occurs while retrieving the users from the group.</exception>
        public async Task<List<UsersToEvaluateDTO>?> GetUsersByGroup(int groupId)
        {
            try
            {
                List<User>? users = await _unitOfWork.UserRepository.GetUsersByGroup(groupId);
                if (users is null)
                {
                    _logger.LogWarning("No users found for group id " + groupId);
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

        /// <summary>
        /// Creates a JWT token for a user with specific claims and a security key.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="firstname">The first name of the user.</param>
        /// <param name="lastname">The last name of the user.</param>
        /// <param name="userRole">The role of the user.</param>
        /// <param name="appSecurityKey">The application security key used to sign the token.</param>
        /// <returns>A JWT token for the user containing their claims.</returns>
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
        /// <param name="signUpDTO">The UserSignUpDTO to extract a User From</param>
        /// <returns>A User</returns>
        private User MapToUser(UserSignUpDTO signUpDTO)
        {
            return new User()
            {
                FirstName = signUpDTO.FirstName,
                LastName = signUpDTO.LastName,
                Email = signUpDTO.Email,
                Password = signUpDTO.Password,
                Role = (UserRole)Enum.Parse(typeof(UserRole), signUpDTO.UserRole),
                Manager = null,
                Group = null,
                GroupId = signUpDTO.GroupId,
                EvaluationsMade = new List<Evaluation>(),
                EvaluationsReceived = new List<Evaluation>(),
                Subordinates = new List<User>()
            };
        }

        /// <summary>
        /// Transforms a UserUpdateDTO to a User Object
        /// </summary>
        /// <param name="updateDTO">The UserSignUpDTO to extract a User From</param>
        /// <returns></returns>
        private async Task<User> MapToUser(UserUpdateDTO updateDTO)
        {
            User? existing = await _unitOfWork.UserRepository.GetAsync(updateDTO.Id);

            if (existing is null)
            {
                throw new EntityNotFoundException("User", "User with id " + updateDTO.Id + " could not be retrieved!");
            }
            return new User()
            {
                Id = updateDTO.Id,
                FirstName = updateDTO.Firstname,
                LastName = updateDTO.Lastname,
                Email = updateDTO.Email,
                Password = !string.IsNullOrEmpty(updateDTO.Password) ? updateDTO.Password : existing.Password,
                Role = Enum.TryParse<UserRole>(updateDTO.Role, out UserRole result) ? result : existing.Role,
                Manager = existing.Manager,
                Group = existing.Group,
                GroupId = updateDTO.GroupId != 0 ? updateDTO.GroupId : existing.GroupId,
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
                    throw new EntityNotFoundException("EvaluationAnswer", "Error while trying to retrieve the evaluation answers for evaluation with id" + item.Id + "!");
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
                else if (currentGroup == null)
                {
                    throw new EntityNotFoundException("Group", "Error while trying to identify the group of the evaluatee!");
                }
                else if (currentCycle == null)
                {
                    throw new EntityNotFoundException("Cycle", "Error while trying to retrieve the evaluation cycle of the evaluation!");
                }
            }
            return pastEvaluationsOfUserDTOs;
        }

        /// <summary>
        /// Maps a list of users to a list of UsersToEvaluateDTO objects, transferring the necessary user details.
        /// </summary>
        /// <param name="users">A list of User objects to be mapped.</param>
        /// <returns>A list of UsersToEvaluateDTO objects containing the user's details (Id, Firstname, Lastname, Email).</returns>
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
