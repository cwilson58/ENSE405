namespace EatingHabitAnalyzerApp.Models;

public class FeelingSurvey
{
    public int SurveyID { get; set; }

    public int UserID { get; set; }

    public string Blurb { get; set; }

    public DateTime SurveyDate { get; set; }

    public int Q1 { get; set; }
    
    public int Q2 { get; set; }
    
    public int Q3 { get; set; }
}
