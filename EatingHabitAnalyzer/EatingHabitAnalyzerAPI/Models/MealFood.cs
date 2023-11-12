using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatingHabitAnalyzerAPI.Models;

public partial class MealFood
{
    public int? MealId { get; set; }

    public string? Barcode { get; set; }

    public int? NumberOfServings { get; set; }

    public int NumberOfGrams { get; set; }

    public int MealFoodId { get; set; }

    [NotMapped]
    public Food? Food { get; set; }
}
