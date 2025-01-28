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

        /// <summary>
        /// Adds the evaluation answers based on the provided submission data and evaluation.
        /// </summary>
        /// <param name="submitEvaluationDTO">The data transfer object containing the answers to be added.</param>
        /// <param name="evaluation">The evaluation to which the answers belong.</param>
        /// <exception cref="Exception">Thrown if there is an error during the process of adding answers.</exception>
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

        /// <summary>
        /// Maps the evaluation answer data transfer object to an actual EvaluationAnswer entity.
        /// </summary>
        /// <param name="evaluationAnswer">The data transfer object containing the answer details.</param>
        /// <param name="evaluation">The evaluation object that the answer will be associated with.</param>
        /// <returns>The mapped EvaluationAnswer entity.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the question referenced by the evaluation answer cannot be found.</exception>
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
