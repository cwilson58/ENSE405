namespace EatingHabitAnalyzerApp.Models;

public class Goal
{
    public int GoalID { get; set; }

    public int GroupID { get; set; }

    public int LostPounds { get; set; }

    public int ExerciseCalories { get; set; }

    public string Custom { get; set; }

    public DateTime CompleteBy { get; set; }
}
