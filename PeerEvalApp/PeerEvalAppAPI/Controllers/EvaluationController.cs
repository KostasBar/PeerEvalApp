using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;
using PeerEvalAppAPI.Exceptions;
using PeerEvalAppAPI.Services;
using Serilog;

namespace PeerEvalAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public EvaluationController(IApplicationService applicationService, IConfiguration configuration,
            IMapper mapper)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Submits a new evaluation based on the provided input data.
        /// </summary>
        /// <param name="submitEvaluationDTO">The DTO containing the details for the evaluation submission.</param>
        /// <returns>Returns an HTTP 200 OK status if the submission is successful. If there is an error, an appropriate HTTP status will be returned.</returns>
        /// <response code="200">Successfully submitted the evaluation.</response>
        /// <response code="400">The submission failed due to bad request (e.g. validation errors).</response>
        /// <response code="404">The requested resource was not found (e.g. related entity missing).</response>
        /// <response code="500">An unexpected error occurred during the submission process.</response>
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitEvaluation([FromBody] SubmitEvaluationDTO submitEvaluationDTO)
        {
            try
            {
                Evaluation evaluation = await _applicationService.EvaluationService.AddEvaluation(submitEvaluationDTO);
                if (evaluation == null) throw new Exception("Something went wrong in adding new evaluation");

                await _applicationService.EvaluationAnswerService.AddEvaluationAnswer(submitEvaluationDTO, evaluation);

                return Ok();
            }
            catch(EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Retrieves evaluations by group and cycle IDs.
        /// </summary>
        /// <param name="groupId">The ID of the group to retrieve evaluations for.</param>
        /// <param name="cycleId">The ID of the cycle to retrieve evaluations for.</param>
        /// <returns>Returns a list of evaluations associated with the specified group and cycle, or an empty list if none are found.</returns>
        /// <response code="200">Successfully retrieved the evaluations.</response>
        /// <response code="400">An error occurred while fetching the evaluations.</response>
        /// <response code="404">The requested evaluations were not found.</response>
        [HttpGet("evaluations-by-group/{groupId}/{cycleId}")]
        public async Task<ActionResult<List<EvalByGroupDTO>>> GetEvaluationsByGroup(int groupId, int cycleId)
        {
            try
            {
                List<EvalByGroupDTO>? evalByGroupDTOs = await _applicationService.EvaluationService.GetEvaluationByGroup(groupId, cycleId);

                if (evalByGroupDTOs == null)
                {
                    return new List<EvalByGroupDTO>();
                }
                return Ok(evalByGroupDTOs);
            }
            catch( EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
    }
}
