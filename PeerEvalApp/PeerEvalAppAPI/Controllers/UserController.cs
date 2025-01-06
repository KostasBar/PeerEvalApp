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
