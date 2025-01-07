using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;
using PeerEvalAppAPI.Exceptions;
using PeerEvalAppAPI.Services;

namespace PeerEvalAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationCycleController :BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public EvaluationCycleController(IApplicationService applicationService, IConfiguration configuration,
            IMapper mapper)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("newcycle")]
        public async Task<IActionResult> SignUp([FromBody] InitiateCycleDTO cycleDTO)
        {
            try
            {
                await _applicationService.EvaluationCycleService.InitializeEvaluationCycle(cycleDTO);
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
    }
}
