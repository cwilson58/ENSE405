using System;
using System.Collections.Generic;

namespace EatingHabitAnalyzerAPI.Models;

public partial class Food
{
    public string Barcode { get; set; } = null!;

    public string FoodName { get; set; } = null!;

    public int ServingSizeInGrams { get; set; }

    public int CaloriesPerServing { get; set; }

    public int ProteinPerServing { get; set; }

    public int CarbsPerServing { get; set; }

    public int TotalSaturatedFatPerServing { get; set; }

    public int TotalUnSaturatedFatPerServing { get; set; }

    public int CholesterolPerServing { get; set; }

    public int SodiumPerServing { get; set; }

    public int CaffeinePerServing { get; set; }

    public int SugarPerServing { get; set; }
}
