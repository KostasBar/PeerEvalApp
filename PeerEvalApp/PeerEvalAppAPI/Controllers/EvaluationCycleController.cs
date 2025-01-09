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
    public class EvaluationCycleController : BaseController
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

        [HttpPost("updatecycle")]
        public async Task<IActionResult> UpdateCycle([FromBody] UpdateCycleDTO cycleDTO)
        {
            try
            {
                if ( 0 > cycleDTO.Status && cycleDTO.Status > 1 && cycleDTO.EndWeek < 0)
                {
                    throw new InvalidArgumentException("EvaluationCycle", "Invalid parameters for EvaluationCycle");
                }
                EvaluationCycle? evaluationCycle = await _applicationService.EvaluationCycleService.UpdateEvaluateCycleAsync(cycleDTO);
                if (evaluationCycle != null)
                {
                    return Ok(evaluationCycle); 
                }
                else
                {
                    return NotFound(new { message = "Evaluation Cycle not found." });
                }
            }
            catch(InvalidArgumentException e)
            {
                return BadRequest(new { message = e.Message });
            }
            catch (EntityNotFoundException e)
            {
                
                return NotFound(new {message = e.Message});
            }
            catch (Exception)
            {

                return BadRequest(new { message = "Something went wrong while trying to update an Evaluation Cycle." });
            }
        }
    }
}
