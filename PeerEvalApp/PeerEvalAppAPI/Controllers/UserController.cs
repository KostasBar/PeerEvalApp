using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;
using PeerEvalAppAPI.Services;
using PeerEvalAppAPI.Exceptions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace PeerEvalAppAPI.Controllers
{
    /// <summary>
    /// Manages user-related operations such as login, registration, and fetching user evaluations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserController(IApplicationService applicationService, IConfiguration configuration,
            IMapper mapper)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="credentials">The user's login credentials.</param>
        /// <returns>A JWT token if authentication is successful.</returns>
        /// <response code="200">Returns the JWT token on successful authentication.</response>
        /// <response code="401">If the credentials are invalid or authentication fails.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogInDTO credentials)
        {
            var user = await _applicationService.UserService.VerifyAndGetUserAsync(credentials);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var userToken = _applicationService.UserService.CreateUserToken(user.Id, user.Email!,
                 user.Role, _configuration["Authentication:SecretKey"]!);

            JwtTokenDTO token = new()
            {
                Token = userToken
            };

            return Ok(new
            {
                token = token
            });
        }

        /// <summary>
        /// Registers a new user with the provided details.
        /// </summary>
        /// <param name="signUpDTO">The user's sign up information.</param>
        /// <returns>A success response if registration is successful.</returns>
        /// <response code="200">If the user is successfully registered.</response>
        /// <response code="409">If a user with the same email or username already exists.</response>
        /// <response code="400">If there is an error in the registration process.</response>
        [HttpPost("register")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpDTO signUpDTO)
        {
            try
            {
                await _applicationService.UserService.SignUpUserAsync(signUpDTO);
                return Ok();
            }
            catch (EntityAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Something went wrong while signing up." });
            }
        }

        /// <summary>
        /// Retrieves evaluations associated with a specific user.
        /// </summary>
        /// <param name="id">The ID of the user whose evaluations are to be retrieved.</param>
        /// <returns>A list of evaluations associated with the user.</returns>
        /// <response code="200">Returns a list of evaluations for the user.</response>
        /// <response code="404">If no user with the specified ID exists.</response>
        /// <response code="500">If an error occurs during the process of retrieving evaluations.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Evaluation>>> GetEvaluationsForUser(int id)
        {
            try
            {
                var evalForUser = await _applicationService.UserService.GetEvaluationsForUserAsync(id);
                return Ok(evalForUser);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { message = "An error occurred while retrieving evaluations." });
            }
        }
    }
}
