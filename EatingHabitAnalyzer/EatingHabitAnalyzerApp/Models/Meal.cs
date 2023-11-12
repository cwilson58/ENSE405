using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatingHabitAnalyzerApp.Models;

public class Meal
{
    public int MealId { get; set; }

    public int? EntryId { get; set; }

    public byte MealNumber { get; set; }

    public ObservableCollection<MealFood> MealFoods { get; set; } = new ();

    public int TotalCalories { get => MealFoods.Sum(x => x.Food.CaloriesPerServing * x.NumberOfServings); }

    public string TotalCaloriesAsText => $"Total Calories: {TotalCalories}";

    public string MealNumberAsText => $"Meal Number: {MealNumber}";

    public override bool Equals(object obj)
    {
        if (obj is Meal meal)
        {
            return MealId == meal.MealId;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return MealId;
    }
}
