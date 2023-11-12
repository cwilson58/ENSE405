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
public class TrackingController : ControllerBase
{
    private readonly ILogger<FoodController> _logger;

    private readonly IDatabaseService _service;

    private HttpClient _client;

    public TrackingController(ILogger<FoodController> logger, IDatabaseService service)
    {
        _logger = logger;
        _service = service;
        _client = new HttpClient();
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
        if (diary == null)
        {
            return NotFound("Diary Entry Not Found");
        }

        var meals = _service.GetMealListByEntryId(diary!.EntryId).GetAwaiter().GetResult();
        if (!meals.Any())
        {
            return Ok(JsonSerializer.Serialize(diary));
        }

        diary.Meals.AddRange(meals);
        diary.Meals.ForEach(meal =>
        {
            var mealFoods = _service.GetMealFoodListByMealId(meal.MealId).GetAwaiter().GetResult();
            meal.MealFoods.AddRange(mealFoods);
        });

        return Ok(JsonSerializer.Serialize(diary));
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

        var newMealId = _service.InsertNewMeal(new Meal
        {
            EntryId = diaryEntry.EntryId,
            MealNumber = mealNumber,
        }).GetAwaiter().GetResult();
       
        return newMealId != null ? Ok(JsonSerializer.Serialize(newMealId)) : BadRequest("An Error Occured Saving the Object");
    }

    [HttpPost, Route("AddFood")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult AddFood(string barcode, int mealId, int numberOfServings)
    {
        //converting from 12 to 13 digit barcode
        if(barcode.Length < 13)
        {
            barcode = barcode.PadLeft(13,'0');
        }
        var user = _service.GetUserAsync(User.FindFirstValue("UserEmail")!).GetAwaiter().GetResult();
        var meal = _service.GetMealById(mealId).GetAwaiter().GetResult();

        if (meal == null) return BadRequest("Invalid Meal Id");

        var food = _service.GetFoodAsync(barcode).GetAwaiter().GetResult();
        if(food == null)
        {
            var backup = _client.GetAsync(@$"https://world.openfoodfacts.net/api/v2/product/{barcode}").GetAwaiter().GetResult();
            var apiFood = JsonSerializer.Deserialize<OpenFoodFactsEntry>(backup.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            if(apiFood == null || apiFood.product == null || apiFood.product.nutriments == null)
            {
                return BadRequest("Invalid Barcode");
            }

            string servingSizeMatch = "1";
            if(apiFood.product.serving_size != null)
            {
                servingSizeMatch = Regex.Match(apiFood.product.serving_size!, @"(\d+[.]{0,1}\d{0,1})(\D{0,})$").Groups[1].Value;
            }
            var ex = _service.InsertNewFood(new Food
            {
                Barcode = apiFood.code,
                FoodName = apiFood.product!.product_name_en,
                ServingSizeInGrams = Convert.ToInt32(servingSizeMatch),
                CaffeinePerServing = Convert.ToInt32(apiFood.product!.nutriments!.caffeine_serving),
                CaloriesPerServing = Convert.ToInt32(apiFood.product!.nutriments!.energy_kcal_serving),
                CarbsPerServing = Convert.ToInt32(apiFood.product!.nutriments!.carbohydrates_serving),
                CholesterolPerServing = Convert.ToInt32(apiFood.product!.nutriments!.cholesterol_serving),
                ProteinPerServing = Convert.ToInt32(apiFood.product!.nutriments!.proteins_serving),
                TotalUnSaturatedFatPerServing = Convert.ToInt32(apiFood.product!.nutriments!.fat_serving),
                TotalSaturatedFatPerServing = Convert.ToInt32(apiFood.product!.nutriments!.saturated_fat_serving),
                SugarPerServing = Convert.ToInt32(apiFood.product!.nutriments!.sugars_serving),
                SodiumPerServing = Convert.ToInt32(apiFood.product!.nutriments!.sodium_serving),
            }).GetAwaiter().GetResult();

            if(ex != null) return BadRequest(ex);

            food = _service.GetFoodAsync(barcode).GetAwaiter().GetResult();
        }

        var insertEx = _service.InsertNewMealFood(new MealFood
        {
            MealId = meal.MealId,
            Barcode = food.Barcode,
            NumberOfServings = numberOfServings,
            NumberOfGrams = food.ServingSizeInGrams * numberOfServings,
        }).GetAwaiter().GetResult();

        if (insertEx != null) return BadRequest(insertEx);

        return Ok();


    }

    [HttpDelete, Route("RemoveFood")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult RemoveFood(int mealFoodId)
    {
        var mealFood = _service.GetMealFoodById(mealFoodId).GetAwaiter().GetResult();
        if(mealFood == null)
        {
            return BadRequest("Invalid Meal Food Id");
        }
        var ex = _service.DeleteMealFood(mealFood).GetAwaiter().GetResult();
        return ex == null ? Ok("Item Removed From Log") : BadRequest();
    }

    [HttpGet, Route("GetMeal")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult GetMeal(int mealId)
    {
        var meal = _service.GetMealById(mealId).GetAwaiter().GetResult();
        if(meal == null) return BadRequest("Invalid Meal Id"); 
        meal.MealFoods.AddRange(_service.GetMealFoodListByMealId(mealId).GetAwaiter().GetResult());
        return Ok(meal);
    }

    [HttpDelete, Route("DeleteMeal")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult DeleteMeal(int mealId)
    {
        var meal = _service.GetMealById(mealId).GetAwaiter().GetResult();
        if (meal == null) return BadRequest("Invalid Meal Id");
        var ex = _service.DeleteMeal(mealId).GetAwaiter().GetResult();
        return ex == null ? Ok(meal) : BadRequest(ex.Message);
    }
}
