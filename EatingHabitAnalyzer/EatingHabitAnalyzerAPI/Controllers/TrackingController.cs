using EatingHabitAnalyzerAPI.Models;
using EatingHabitAnalyzerAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EatingHabitAnalyzerAPI.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class TrackingController : ControllerBase
{
    private readonly ILogger<FoodController> _logger;

    private readonly IDatabaseService _service;

    public TrackingController(ILogger<FoodController> logger, IDatabaseService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet, Route("GetDiary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DiaryEntry> GetDiary(DateTime date)
    {
        var user = _service.GetUserAsync(User.FindFirstValue("UserEmail")!).GetAwaiter().GetResult();
        if (user == null)
        {
            return NotFound("User Not Found");
        }
        var diary = _service.GetDirayEntryByDateAndUserId(date, user.UserId).GetAwaiter().GetResult();
        return diary == null ? NotFound() : Ok(diary);
    }

    [HttpPost, Route("NewDiary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult NewDiary(DateTime date)
    {
        var user = _service.GetUserAsync(User.FindFirstValue("UserEmail")!).GetAwaiter().GetResult();
        if (user == null)
        {
            return BadRequest("User Not Found");
        }

        var ex = _service.InsertNewDiaryEntry(new DiaryEntry
        {
            EntryDateTime = date,
            IsComplete = false,
            UserId = user.UserId
        }).GetAwaiter().GetResult();
        return  ex == null ? Ok() : BadRequest(ex);
    }

    [HttpPost, Route("AddMeal")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult AddMeal(byte mealNumber, DateTime? date = null,int? entryId = null)
    {
        if(date == null && entryId == null)
        {
            return BadRequest("One of the two parameters must be not null");
        }

        var user = _service.GetUserAsync(User.FindFirstValue("UserEmail")!).GetAwaiter().GetResult();
        DiaryEntry? diaryEntry;
        if(date == null)
        {
            diaryEntry = _service.GetDiaryEntryById(entryId!.Value).GetAwaiter().GetResult();
        }
        else
        {
            diaryEntry = _service.GetDirayEntryByDateAndUserId(date.Value, user.UserId).GetAwaiter().GetResult();
        }

        if (diaryEntry == null)
        {
            return BadRequest("Diary Entry Not Found");
        }

        
        var ex = _service.InsertNewMeal(new Meal
        {
            EntryId = diaryEntry.EntryId,
            MealNumber = mealNumber,
        }).GetAwaiter().GetResult();
        
        return ex != null ? Ok("New Meal Added") : BadRequest(ex);
    }

}
