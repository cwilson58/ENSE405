namespace EatingHabitAnalyzerAPI.Models;

public class GoalEntry
{
    public int GoalEntryID { get; set; }

    public int GroupID { get; set; }

    public int UserID { get; set; }

    public int LostPounds { get; set; }

    public int ExerciseCalories { get; set; }

    public bool CustomComplete { get; set; }

}
