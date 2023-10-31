using System;
using System.Collections.Generic;

namespace EatingHabitAnalyzerAPI.Models;

public partial class Meal
{
    public int MealId { get; set; }

    public int? EntryId { get; set; }

    public byte MealNumber { get; set; }
}
