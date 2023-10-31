using System;
using System.Collections.Generic;

namespace EatingHabitAnalyzerAPI.Models;

public partial class MealFood
{
    public int? MealId { get; set; }

    public string? Barcode { get; set; }

    public int? NumberOfServings { get; set; }

    public int NumberOfGrams { get; set; }

    public int MealFoodId { get; set; }
}
