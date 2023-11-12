using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatingHabitAnalyzerApp.Models;

public class MealFood
{
    public int MealId { get; set; }
    public string Barcode { get; set; }
    public int NumberOfServings { get; set; }
    public int NumberOfGrams { get; set; }
    public int MealFoodId { get; set; }
    public Food Food { get; set; }
    public string FoodAsString => $"{Food.FoodName}";
    public string TotalCaloriesAsString => $"{Food.CaloriesPerServing * NumberOfServings}";
}
