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
