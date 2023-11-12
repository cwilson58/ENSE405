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

    //Overloads to support built in methods
    public override bool Equals(object obj)
    {
        if(obj is MealFood mealFood)
        {
            return mealFood.Barcode == Barcode;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Barcode.GetHashCode();
    }

    public bool Equals(MealFood other)
    {
        return other.Barcode == Barcode;
    }
}
