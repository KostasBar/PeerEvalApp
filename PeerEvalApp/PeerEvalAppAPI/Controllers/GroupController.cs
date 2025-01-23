using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PeerEvalAppAPI.DTO;
using PeerEvalAppAPI.DTO.UserDTOs;
using PeerEvalAppAPI.Exceptions;
using PeerEvalAppAPI.Services;

namespace PeerEvalAppAPI.Controllers
{
    public class GroupController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GroupController(IApplicationService applicationService, IConfiguration configuration,
            IMapper mapper)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GroupReturnDTO>?>> GetAllGroups()
        {
            try
            {
                List<GroupReturnDTO>? groupReturns = await _applicationService.GroupService.GetAllGroupsAsync();
                if (groupReturns == null)
                {
                    throw new Exception();
                }
                return Ok(groupReturns);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception)
            {

                return BadRequest(new { message = "Something went wrong while retrieving groups." });
            }
        }
    }
}
