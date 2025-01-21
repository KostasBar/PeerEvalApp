using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO.EvaluationCycleDTOs;
using PeerEvalAppAPI.Exceptions;
using PeerEvalAppAPI.Services;

namespace PeerEvalAppAPI.Controllers
{
    /// <summary>
    /// Handles all operations related to evaluation cycles such as creating and updating cycles.
    /// </summary>
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

        /// <summary>
        /// Initiates a new evaluation cycle with the specified details.
        /// </summary>
        /// <param name="cycleDTO">The evaluation cycle data transfer object containing all necessary information.</param>
        /// <returns>Returns a successful response if the cycle was created.</returns>
        /// <response code="200">The evaluation cycle was successfully created.</response>
        /// <response code="409">An evaluation cycle with the same properties already exists.</response>
        /// <response code="400">There was an error during the creation of the evaluation cycle.</response>
        [HttpPost("newcycle")]
        public async Task<IActionResult> InitializeCycle([FromBody] InitiateCycleDTO cycleDTO)
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
            catch(OpenCycleAlreadyExists e)
            {
                return Conflict(new {message = "There can be only one Evaluation Cycle open each time!"});
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Something went wrong while signing up." });
            }
        }

        /// <summary>
        /// Updates an existing evaluation cycle with new information.
        /// </summary>
        /// <param name="cycleDTO">The update data for the evaluation cycle.</param>
        /// <returns>Returns a successful response if the cycle was found and updated.</returns>
        /// <response code="200">The evaluation cycle was successfully updated and returned.</response>
        /// <response code="404">The evaluation cycle to update was not found.</response>
        /// <response code="400">There was a problem with the input parameters or during the update process.</response>
        [HttpPost("updatecycle")]
        public async Task<IActionResult> UpdateCycle([FromBody] UpdateCycleDTO cycleDTO)
        {
            try
            {
                if (0 > cycleDTO.Status && cycleDTO.Status > 1 && cycleDTO.EndWeek < 0)
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
            catch (InvalidArgumentException e)
            {
                return BadRequest(new { message = e.Message });
            }
            catch (EntityNotFoundException e)
            {

                return NotFound(new { message = e.Message });
            }
            catch (Exception)
            {

                return BadRequest(new { message = "Something went wrong while trying to update an Evaluation Cycle." });
            }
        }

        [HttpGet("evaluationCycleExists")]
        public async Task<IActionResult> evaluationCycleExists()
        {
            try
            {
                EvaluationCycle? exists = await _applicationService.EvaluationCycleService.EvaluationCycleExists();
                if (exists != null)
                {
                    return Ok(exists.Id);
                }

                return Ok(0);

            }
            catch (Exception)
            {

                return BadRequest(new { message = "Something went wrong while trying to retrieve open Evaluation Cycle." });
            }
        }

        [HttpGet("get-all-cycles")]
        public async Task<IActionResult> GetAllEvaluationCycles()
        {
            try
            {
                List<EvaluationCycle>? evaluationCycles = await _applicationService.EvaluationCycleService.GetEvaluationCyclesAsync();

                return Ok(evaluationCycles);

            }
            catch (Exception)
            {

                return BadRequest(new { message = "Something went wrong while trying to retrieve open Evaluation Cycle." });
            }
        }
    }
}
