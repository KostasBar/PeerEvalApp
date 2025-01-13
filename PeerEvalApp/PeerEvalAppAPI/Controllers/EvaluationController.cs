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
                Evaluation evaluation = await _applicationService.EvaluationService.AddEvaluation(submitEvaluationDTO);
                if (evaluation == null) throw new Exception();

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
    }
}
