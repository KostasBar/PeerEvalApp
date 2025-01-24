using AutoMapper;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;
using PeerEvalAppAPI.Exceptions;
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
                List<GroupReturnDTO>? groupReturnDTOs = groups?.Select(g => MapToGroupReturnDTO(g)).ToList();
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

        public async Task AddGroup(string groupName)
        {
            try{   
                
                // Check if group name already exists
                List<Group>? groups = (List<Group>?)await _unitOfWork.GroupRepository.GetAllAsync();
                if (groups is not null){
                    bool exists = groups.Any(g => g.GroupName.Equals(groupName, StringComparison.OrdinalIgnoreCase));
                    if (exists)
                    {
                        throw new EntityAlreadyExistsException("Group", "Group " + groupName + " already exists.");
                    }
                }

                Group group = MapToGroup(groupName);
                await _unitOfWork.GroupRepository.AddAsync(group);
                await _unitOfWork.SaveAsync();
                
            }
            catch (EntityAlreadyExistsException e)
            {
                _logger.LogWarning(e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError("An error occured while trying to add a new group.");
                throw;
            }
        }

        private GroupReturnDTO MapToGroupReturnDTO(Group group)
        {
            return  new GroupReturnDTO()
            {
                Id = group.Id,
                GroupName = group.GroupName
            };
        }

        private Group MapToGroup(string groupName)
        {
            return new Group()
            {
                GroupName = groupName

            };
        }
    }
}
