using EatingHabitAnalyzerAPI.Models;
using EatingHabitAnalyzerAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EatingHabitAnalyzerAPI.Controllers;



[Authorize]
[ApiController]
[Route("[controller]")]
//NOTE: The user is stored in the JWT token and must be authorized to use this, so we should NEVER take in a user id for these endpoints.
public class ProfileController : ControllerBase
{
    private readonly ILogger<FoodController> _logger;

    private readonly IDatabaseService _service;

    private HttpClient _client;

    public ProfileController(ILogger<FoodController> logger, IDatabaseService service)
    {
        _logger = logger;
        _service = service;
        _client = new HttpClient();
    }

    [HttpGet, Route("GetProfile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<User> GetProfile()
    {
        var user = _service.GetUserAsync(User.FindFirstValue("UserEmail")!).GetAwaiter().GetResult();
        if (user == null)
        {
            return NotFound("User Not Found");
        }
        user.Pin = null;
        return Ok(JsonSerializer.Serialize(user));
    }

    [HttpPatch, Route("TrackNewWeight")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult TrackNewWeight(decimal weight)
    {

        //TODO in the future this should track a history of weights

        var user = _service.GetUserAsync(User.FindFirstValue("UserEmail")!).GetAwaiter().GetResult();
        if (user == null)
        {
            return BadRequest("User Not Found");
        }

        user.WeightInPounds = weight;

        var ex = _service.UpdateUser(user).GetAwaiter().GetResult();
        return ex == null ? Ok() : BadRequest(ex);
    }

    [HttpPatch, Route("SetCalorieGoal")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult SetCalorieGoal(int goal)
    {
        var user = _service.GetUserAsync(User.FindFirstValue("UserEmail")!).GetAwaiter().GetResult();
        if (user == null)
        {
            return BadRequest("User Not Found");
        }

        user.GoalDailyCalories = goal;

        var ex = _service.UpdateUser(user).GetAwaiter().GetResult();
        return ex == null ? Ok() : BadRequest(ex);
    }

    [HttpPatch, Route("SetWeightGoal")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult SetCalorieGoal(decimal goal)
    {
        var user = _service.GetUserAsync(User.FindFirstValue("UserEmail")!).GetAwaiter().GetResult();
        if (user == null)
        {
            return BadRequest("User Not Found");
        }

        user.GoalWeight = goal;

        var ex = _service.UpdateUser(user).GetAwaiter().GetResult();
        return ex == null ? Ok() : BadRequest(ex);
    }
}
