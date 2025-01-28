using System.Text.RegularExpressions;
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

        /// <summary>
        /// Retrieves a list of all groups.
        /// </summary>
        /// <returns>Returns a list of group details. If no groups are found, returns an empty list.</returns>
        /// <response code="200">Successfully retrieved the list of groups.</response>
        /// <response code="400">An error occurred while trying to retrieve the list of groups.</response>
        /// <response code="404">The requested groups could not be found.</response>
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

        /// <summary>
        /// Adds a new group to the system.
        /// </summary>
        /// <param name="groupName">The name of the new group to be added.</param>
        /// <returns>Returns a success status if the group is added successfully.</returns>
        /// <response code="200">Successfully added the new group.</response>
        /// <response code="404">The group already exists in the system.</response>
        /// <response code="500">An error occurred while trying to add the new group.</response>
        [HttpPost]
        public async Task<IActionResult> AddGroup([FromBody] NewGroupDTO groupName)
        {
            try
            {
                await _applicationService.GroupService.AddGroup(groupName.GroupName);
                return Ok();
            }
            catch (EntityAlreadyExistsException e)
            {
                return StatusCode(404, new {message = e.Message});
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while trying to add the new group." });
            }
        }
    }
}
