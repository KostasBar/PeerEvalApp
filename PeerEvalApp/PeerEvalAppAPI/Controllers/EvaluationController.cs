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

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitEvaluation([FromBody] SubmitEvaluationDTO submitEvaluationDTO)
        {
            try
            {
                ILogger<EvaluationController> _logger;
                _logger = new LoggerFactory().AddSerilog().CreateLogger<EvaluationController>();
                _logger.LogCritical(submitEvaluationDTO.ToString());
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
