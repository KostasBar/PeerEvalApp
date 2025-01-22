using Microsoft.AspNetCore.Mvc;
using PeerEvalAppAPI.DTO;
using PeerEvalAppAPI.DTO.UserDTOs;
using PeerEvalAppAPI.Exceptions;
using PeerEvalAppAPI.Services;

namespace PeerEvalAppAPI.Controllers
{
    public class GroupController : BaseController
    {
        protected GroupController(IApplicationService applicationService) : base(applicationService)
        {
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
