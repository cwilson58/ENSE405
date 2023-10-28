using Microsoft.AspNetCore.Mvc;
using EatingHabitAnalyzerAPI.Models;
using EatingHabitAnalyzerAPI.Service;

namespace EatingHabitAnalyzerAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FoodController : ControllerBase
{
    private readonly ILogger<FoodController> _logger;

    private readonly IDatabaseService _service;

    public FoodController(ILogger<FoodController> logger, IDatabaseService service)
    {
        _logger = logger;
        _service = service;
    }
    #region Database Operations

    [HttpGet, Route("GetFood")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Food> Get(string barcode)
    {
        var food = _service.GetFoodAsync(barcode).GetAwaiter().GetResult();
        return food == null ? NotFound() : food;
    }

    [HttpPost,Route("InsertFood")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult Post(Food food)
    {
        var result = _service.InsertNewFood(food).GetAwaiter().GetResult();
        return result == null ? Ok() : BadRequest(result.Message);
    }

    [HttpPost,Route("InsertFoods")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult Post(List<Food> foods)
    {
        return _service.InsertFoods(foods).GetAwaiter().GetResult() == null ? Ok() : BadRequest();
    }

    [HttpPut, Route("UpdateFood")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult Put(Food food)
    {
        var result = _service.UpdateFood(food).GetAwaiter().GetResult();
        return result == null ? Ok() : BadRequest(result.Message);
    }

    [HttpDelete, Route("DeleteFood")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult Delete(string barcode)
    {
        var result = _service.DeleteFood(barcode).GetAwaiter().GetResult();
        return result == null ? Ok() : BadRequest(result.Message);
    }


    #endregion


}
