namespace EatingHabitAnalyzerApp.Models;

public class Goal
{
    public int GoalID { get; set; }


    public int GroupID { get; set; }

    public int? LostPounds { get; set; }

    public string LostPoundsAsString => LostPounds == null || LostPounds == 0 ? "" : $"Lose: {LostPounds}lbs";

    public int? ExerciseCalories { get; set; }

    public string ExerciseCaloriesAsString => ExerciseCalories == null || ExerciseCalories == 0 ? "" : $"Burn: {ExerciseCalories} Calories";

    public string Custom { get; set; }

    public DateTime CompleteBy { get; set; }

    public string CompleteByString => $"Complete This by: {CompleteBy:yyyy-MMMM-dd}";
}
