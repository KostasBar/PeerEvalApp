using AutoMapper;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;
using PeerEvalAppAPI.Exceptions;
using PeerEvalAppAPI.Repositories;
using Serilog;

namespace PeerEvalAppAPI.Services
{
    public class EvaluationCycleService : IEvaluationCycleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public EvaluationCycleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = new LoggerFactory().AddSerilog().CreateLogger<UserService>();
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
                    throw new EntityAlreadyExistsException("EvaluationCycle", "There is already an Evaluation Cycle opened!");
 
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
    }
}
