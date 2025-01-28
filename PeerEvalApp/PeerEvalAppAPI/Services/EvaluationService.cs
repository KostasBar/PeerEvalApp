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

        /// <summary>
        /// Adds a new evaluation based on the provided submission data.
        /// </summary>
        /// <param name="submitEvaluationDTO">The data transfer object containing the details of the evaluation to be added.</param>
        /// <returns>The created evaluation entity.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if any referenced entities (user or evaluation cycle) are not found.</exception>
        /// <exception cref="Exception">Thrown for any errors that occur during the process of adding the evaluation.</exception>
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

        /// <summary>
        /// Retrieves evaluations for a specific group and evaluation cycle.
        /// </summary>
        /// <param name="groupID">The ID of the group for which evaluations are to be retrieved.</param>
        /// <param name="cycleId">The ID of the evaluation cycle for which evaluations are to be retrieved.</param>
        /// <returns>A list of evaluations for the specified group and evaluation cycle.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if no evaluations are found for the given group and cycle.</exception>
        /// <exception cref="Exception">Thrown for any errors that occur while retrieving evaluations by group.</exception>
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

        /// <summary>
        /// Maps the submission data transfer object to an evaluation entity.
        /// </summary>
        /// <param name="submitEvaluationDTO">The data transfer object containing the details for the evaluation.</param>
        /// <returns>The mapped evaluation entity.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if any referenced entities (user or evaluation cycle) are not found.</exception>
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
