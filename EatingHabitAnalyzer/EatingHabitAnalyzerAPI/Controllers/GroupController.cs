using Microsoft.AspNetCore.Mvc;
using EatingHabitAnalyzerAPI.Models;
using EatingHabitAnalyzerAPI.Service;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EatingHabitAnalyzerAPI.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class GroupController : ControllerBase
{
    private readonly ILogger<FoodController> _logger;

    private readonly IDatabaseService _service;

    public GroupController(ILogger<FoodController> logger, IDatabaseService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet, Route("GetGroup")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Group> GetGroup(int groupID)
    {
        var group = _service.GetGroupById(groupID).GetAwaiter().GetResult();
        if (group == null)
        {
            return NotFound("Group Not Found");
        }
        return Ok(group);
    }

    [HttpGet, Route("GetUserGroups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Group>> GetUserGroups()
    {

        var user = _service.GetUserAsync(User.FindFirstValue("UserEmail")!).GetAwaiter().GetResult();
        if (user == null)
        {
            return BadRequest("User Not Found");
        }

        var groups = _service.GetGroupsByUserId(user.UserId).GetAwaiter().GetResult();
        if (!groups.Any())
        {
            return NotFound("Group Not Found");
        }
        return Ok(groups);
    }

    [HttpPost, Route("JoinGroup")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult JoinGroup(int groupId)
    {

        var user = _service.GetUserAsync(User.FindFirstValue("UserEmail")!).GetAwaiter().GetResult();
        if (user == null)
        {
            return BadRequest("User Not Found");
        }

        var ex = _service.InsertNewGroupMember(new GroupMember { GroupID = groupId, UserID = user.UserId }).GetAwaiter().GetResult();
        
        if(ex != null)
        {
            return BadRequest(ex);
        }
        
        return Ok();
    }

    [HttpPost, Route("CreateGoal")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult CreatGoal(Goal goal)
    {

        var user = _service.GetUserAsync(User.FindFirstValue("UserEmail")!).GetAwaiter().GetResult();
        if (user == null)
        {
            return BadRequest("User Not Found");
        }

        var ex = _service.InsertNewGroupGoal(goal).GetAwaiter().GetResult();
        if(ex != null)
        {
            return BadRequest(ex);
        }
        return Ok();

    }

    [HttpPost, Route("CreateGoalEntry")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult CreatGoalEntry(GoalEntry entry)
    {

        var user = _service.GetUserAsync(User.FindFirstValue("UserEmail")!).GetAwaiter().GetResult();
        if (user == null)
        {
            return BadRequest("User Not Found");
        }

        var ex = _service.InsertNewGoalEntrie(entry).GetAwaiter().GetResult();
        if (ex != null)
        {
            return BadRequest(ex);
        }
        return Ok();

    }
}
