using AutoMapper;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;
using PeerEvalAppAPI.Exceptions;
using PeerEvalAppAPI.Repositories;
using Serilog;

namespace PeerEvalAppAPI.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EvaluationService> _logger;

        public EvaluationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = new LoggerFactory().AddSerilog().CreateLogger<EvaluationService>();
        }

        public async Task<Evaluation> AddEvaluation(SubmitEvaluationDTO submitEvaluationDTO)
        {
            Evaluation evaluation;
            try
            {
                evaluation = await MapToEvaluation(submitEvaluationDTO);

                await _unitOfWork.EvaluationRepository.AddAsync(evaluation);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Evaluation of User with id " + submitEvaluationDTO.EvaluatorUserId +
                    " for User with id " + submitEvaluationDTO.EvaluateeUserId + " successfully submited!");

                return evaluation;

            }
            catch(EntityNotFoundException e)
            {
                _logger.LogWarning(e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<List<EvalByGroupDTO>?> GetEvaluationByGroup(int groupID, int cycleId)
        {
            try
            {
                List<EvalByGroupDTO>? evaluations = await _unitOfWork.EvaluationRepository.GetEvaluationByGroup(groupID, cycleId);
                if (evaluations == null)
                {
                    throw new EntityNotFoundException("Evaluations", "No Evaluations Found");
                }
                return evaluations;
            }
            catch(EntityNotFoundException e)
            {
                _logger.LogError(e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while trying to retrieve evaluations by group!");
                throw;
            }
        }

        public async Task<Evaluation> MapToEvaluation(SubmitEvaluationDTO submitEvaluationDTO)
        {
            User? evaluatee = await _unitOfWork.UserRepository.GetUserById(submitEvaluationDTO.EvaluateeUserId);
            if (evaluatee == null)
            {
                throw new EntityNotFoundException("User", "Evaluatee User with id " + submitEvaluationDTO.EvaluateeUserId + " could not be found!");
            }
            User? evaluator = await _unitOfWork.UserRepository.GetUserById(submitEvaluationDTO.EvaluatorUserId);
            if (evaluator == null)
            {
                throw new EntityNotFoundException("User", "Evaluator User with id " + submitEvaluationDTO.EvaluatorUserId + " could not be found!");
            }
            EvaluationCycle? evaluationCycle = await _unitOfWork.EvaluationCycleRepository.GetAsync(submitEvaluationDTO.EvaluationCycleId);
            if (evaluationCycle == null)
            {
                throw new EntityNotFoundException("EvaluationCycle", "Evaluation Cycle with id " + submitEvaluationDTO.EvaluationCycleId + " could not be found!");
            }
            return new Evaluation
            {
                EvaluateeUserId = submitEvaluationDTO.EvaluateeUserId,
                EvaluatorUserId = submitEvaluationDTO.EvaluatorUserId,
                EvaluationCycleId = submitEvaluationDTO.EvaluationCycleId,
                TimeStamp = submitEvaluationDTO.TimeStamp,
                EvaluateeUser = evaluatee,
                EvaluatorUser = evaluator,
                EvaluationCycle = evaluationCycle,
                Answers = null
            };
        }
    }
}
