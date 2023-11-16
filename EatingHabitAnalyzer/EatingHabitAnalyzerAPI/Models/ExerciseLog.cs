namespace EatingHabitAnalyzerAPI.Models;

public class ExerciseLog
{
    public int LogID { get; set; }

    public int UserID { get; set; }

    public DateTime LogDate { get; set; }

    public int CaloriesBurned { get; set; }
}
