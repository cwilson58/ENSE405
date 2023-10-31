namespace EatingHabitAnalyzerAPI.Models;

public class VerboseMeal
{
    public int MealId { get; set; }

    public int? EntryId { get; set; }

    public byte MealNumber { get; set; }

    public List<(int servings,Food food)> MealFoods { get; set; } = new();
}

