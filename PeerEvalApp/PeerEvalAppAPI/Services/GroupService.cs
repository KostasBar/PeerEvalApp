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

        /// <summary>
        /// Retrieves all groups from the data source and maps them to a list of GroupReturnDTOs.
        /// </summary>
        /// <returns>A list of GroupReturnDTOs representing all groups in the system.</returns>
        /// <exception cref="Exception">Thrown if an error occurs while retrieving the groups from the data source.</exception>
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

        /// <summary>
        /// Adds a new group to the system if a group with the same name does not already exist.
        /// </summary>
        /// <param name="groupName">The name of the group to be added.</param>
        /// <exception cref="EntityAlreadyExistsException">Thrown if a group with the same name already exists in the system.</exception>
        /// <exception cref="Exception">Thrown for any errors that occur while trying to add the new group.</exception>
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

        /// <summary>
        /// Maps a Group entity to a GroupReturnDTO.
        /// </summary>
        /// <param name="group">The Group entity to be mapped.</param>
        /// <returns>The corresponding GroupReturnDTO.</returns>
        private GroupReturnDTO MapToGroupReturnDTO(Group group)
        {
            return  new GroupReturnDTO()
            {
                Id = group.Id,
                GroupName = group.GroupName
            };
        }

        /// <summary>
        /// Maps a group name to a new Group entity.
        /// </summary>
        /// <param name="groupName">The name of the group to be mapped.</param>
        /// <returns>The corresponding Group entity.</returns>
        private Group MapToGroup(string groupName)
        {
            return new Group()
            {
                GroupName = groupName

            };
        }
    }
}
