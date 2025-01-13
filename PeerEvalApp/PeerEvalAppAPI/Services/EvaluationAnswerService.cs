using AutoMapper;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;
using PeerEvalAppAPI.Exceptions;
using PeerEvalAppAPI.Repositories;
using Serilog;

namespace PeerEvalAppAPI.Services
{
    public class EvaluationAnswerService : IEvaluationAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EvaluationAnswerService> _logger;

        public EvaluationAnswerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = new LoggerFactory().AddSerilog().CreateLogger<EvaluationAnswerService>();
        }

        public async Task AddEvaluationAnswer(SubmitEvaluationDTO submitEvaluationDTO, Evaluation evaluation)
        {
            List<EvaluationAnswerDTO> answerDTOs = submitEvaluationDTO.Answers;
            try
            {
                foreach (EvaluationAnswerDTO eaDTO in answerDTOs)
                {
                    EvaluationAnswer ea = await MapToEvaluationAnswer(eaDTO, evaluation);
                    await _unitOfWork.EvaluationAnswerRepository.AddAsync(ea);
                    await _unitOfWork.SaveAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<EvaluationAnswer> MapToEvaluationAnswer(EvaluationAnswerDTO evaluationAnswer, Evaluation evaluation)
        {
            Question? q = await _unitOfWork.QuestionRepository.GetAsync(evaluationAnswer.QuestionId);
            if (q == null)
            {
                throw new EntityNotFoundException("Question", "Question with id "+ evaluationAnswer.QuestionId +" could not be found!");
            }
            return new EvaluationAnswer()
            {
                AnswerValue = evaluationAnswer.AnswerValue,
                QuestionId = evaluationAnswer.QuestionId,
                Question = q,
                Evaluation = evaluation
            };
        }
    }
}
