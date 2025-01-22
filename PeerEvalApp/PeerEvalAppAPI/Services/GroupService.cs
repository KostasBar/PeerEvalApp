using AutoMapper;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;
using PeerEvalAppAPI.Repositories;
using Serilog;

namespace PeerEvalAppAPI.Services
{
    public class GroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EvaluationAnswerService> _logger;

        public GroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = new LoggerFactory().AddSerilog().CreateLogger<EvaluationAnswerService>();
        }

        public async Task<List<GroupReturnDTO>> GetAllGroupsAsync()
        {
            try
            {
                List<Group>? groups = (List<Group>?)await _unitOfWork.GroupRepository.GetAllAsync();
                List<GroupReturnDTO>? groupReturnDTOs = groups?.Select(g => _mapper.Map<GroupReturnDTO>(g)).ToList();
                if (groupReturnDTOs is null)
                {
                    List<GroupReturnDTO> groupReturns = new List<GroupReturnDTO>();
                    return groupReturns;
                }
                return groupReturnDTOs;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while retrieving Groups");
                throw;
            }
        }
    }
}
