using AutoMapper;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO.EvaluationCycleDTOs;
using PeerEvalAppAPI.Exceptions;
using PeerEvalAppAPI.Repositories;
using Serilog;

namespace PeerEvalAppAPI.Services
{
    public class EvaluationCycleService : IEvaluationCycleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EvaluationService> _logger;

        public EvaluationCycleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = new LoggerFactory().AddSerilog().CreateLogger<EvaluationService>();
        }

        /// <summary>
        /// Retrieves an evaluation cycle by its ID.
        /// </summary>
        /// <param name="id">The ID of the evaluation cycle to retrieve.</param>
        /// <returns>The evaluation cycle if found, otherwise throws an exception.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the evaluation cycle with the specified ID is not found.</exception>
        /// <exception cref="Exception">Thrown for other errors that occur while retrieving the evaluation cycle.</exception>
        public async Task<EvaluationCycle?> GetEvaluationCycleAsync(int id)
        {
            EvaluationCycle? evaluationCycle = null;
            try
            {
                evaluationCycle = await _unitOfWork.EvaluationCycleRepository.GetAsync(id);

                if (evaluationCycle == null) throw new EntityNotFoundException("EvaluationCycle", "Evaluation Cycle with id "+ id + " not found!");
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogWarning(e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while retrieving Evaluation Cycle!");
                throw;
            }

            return evaluationCycle;
        }

        /// <summary>
        /// Initializes a new evaluation cycle with the provided data.
        /// </summary>
        /// <param name="initiateCycleDTO">The data transfer object containing the details to initialize the new evaluation cycle.</param>
        /// <exception cref="OpenCycleAlreadyExists">Thrown if an open evaluation cycle already exists.</exception>
        /// <exception cref="Exception">Thrown for errors that occur during initialization of the evaluation cycle.</exception>
        public async Task InitializeEvaluationCycle(InitiateCycleDTO initiateCycleDTO)
        {
            EvaluationCycle evaluationCycle = MapToEvaluationCycle(initiateCycleDTO);

            try
            {
                if (await _unitOfWork.EvaluationCycleRepository.OpenCycleExists())
                {
                    throw new OpenCycleAlreadyExists("EvaluationCycle", "There is already an Evaluation Cycle opened!");
 
                }

                await _unitOfWork.EvaluationCycleRepository.AddAsync(evaluationCycle);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Evaluation Cycle with Title: {Tile} initialized successfully.", evaluationCycle.Title);
            }
            catch (EntityAlreadyExistsException e)
            {
                _logger.LogWarning(e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while initializing Evaluation Cycle!");
                throw;
            }
        }

        /// <summary>
        /// Updates an existing evaluation cycle with the provided data.
        /// </summary>
        /// <param name="updateCycleDTO">The data transfer object containing the new details for the evaluation cycle.</param>
        /// <returns>The updated evaluation cycle entity.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the evaluation cycle with the specified ID is not found.</exception>
        /// <exception cref="Exception">Thrown for errors that occur during the update process.</exception>
        public async Task<EvaluationCycle?> UpdateEvaluateCycleAsync(UpdateCycleDTO updateCycleDTO)
        {
            EvaluationCycle? evaluationCycle, newEvaluationCycle;
            try
            {   
                evaluationCycle = await MapToEvaluationCycle(updateCycleDTO);
                newEvaluationCycle = await _unitOfWork.EvaluationCycleRepository.UpdateEvaluationCycleAsync(evaluationCycle);
                
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogWarning(e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while trying to update Evaluation Cycle with id " + updateCycleDTO.Id);
                throw;
            }
            return evaluationCycle;
        }

        /// <summary>
        /// Checks if an open evaluation cycle exists.
        /// </summary>
        /// <returns>The currently open evaluation cycle, or null if no open cycle exists.</returns>
        /// <exception cref="Exception">Thrown for errors while trying to retrieve the open evaluation cycle.</exception>
        public async Task<EvaluationCycle?> EvaluationCycleExists()
        {
            try
            {
                if (await _unitOfWork.EvaluationCycleRepository.OpenCycleExists())
                {
                    EvaluationCycle? existing = await _unitOfWork.EvaluationCycleRepository.GetOpenEvaluationCycle();
                    return existing;

                }
                return null;
            }
            catch (Exception e)
            {

                _logger.LogError(e, "Error while trying to retrieve open Evaluation Cycle ");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a list of all evaluation cycles.
        /// </summary>
        /// <returns>A list of evaluation cycles, or an empty list if no cycles are found.</returns>
        /// <exception cref="Exception">Thrown for errors while trying to retrieve the list of evaluation cycles.</exception>
        public async Task<List<EvaluationCycle>?> GetEvaluationCyclesAsync()
        {
            try
            {
                List<EvaluationCycle>? evaluationCycles = (List<EvaluationCycle>?)await _unitOfWork.EvaluationCycleRepository.GetAllAsync();

                if(evaluationCycles is null)
                {
                    return new List<EvaluationCycle>();
                }

                return evaluationCycles;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while trying to retrieve list of Evaluation Cycles.");
                throw;
            }
        }

        /// <summary>
        /// Maps the initiate cycle data transfer object to an evaluation cycle entity.
        /// </summary>
        /// <param name="cycleDTO">The data transfer object containing the details for the new evaluation cycle.</param>
        /// <returns>The mapped evaluation cycle entity.</returns>
        public EvaluationCycle MapToEvaluationCycle(InitiateCycleDTO cycleDTO)
        {
            return new EvaluationCycle()
            {
                Title = cycleDTO.Title,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(cycleDTO.Weeks * 7),
                Status = 0
            };
        }

        /// <summary>
        /// Maps the update cycle data transfer object to an evaluation cycle entity.
        /// </summary>
        /// <param name="cycleDTO">The data transfer object containing the updated details for the evaluation cycle.</param>
        /// <returns>The mapped evaluation cycle entity with the updated details.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the evaluation cycle with the specified ID is not found.</exception>
        public async Task<EvaluationCycle> MapToEvaluationCycle(UpdateCycleDTO cycleDTO)
        {
            EvaluationCycle? existing = await _unitOfWork.EvaluationCycleRepository.GetAsync(cycleDTO.Id);
            if (existing is null)
            {
                throw new EntityNotFoundException("EvaluationCycle", "Evaluation Cycle with Id " + cycleDTO.Id + " not found!");
            }
            //New date is the old date postponed by x number of weeks
            DateTime newEndDate = cycleDTO.EndWeek > 0 ? existing.EndDate.AddDays(cycleDTO.EndWeek * 7) : existing.EndDate;
            return new EvaluationCycle()
            {
                Id = existing.Id,
                Title = existing.Title,
                StartDate = existing.StartDate,
                EndDate = newEndDate,
                Status = cycleDTO.Status,

            };
        }
    }
}
