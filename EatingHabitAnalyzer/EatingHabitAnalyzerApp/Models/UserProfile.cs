using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EatingHabitAnalyzerApp.Models;

public class UserProfile
{
    public int UserId { get; set; }

    public string Name { get; set; }

    public int HeightInInches { get; set; }

    public double WeightInPounds { get; set; }

    public DateTime DateOfBirth { get; set; }

    public double GoalWeight { get; set; }

    public int GoalDailyCalories { get; set; }

    public string Email { get; set; }

    public string? Pin { get; set; }

    public Array diaryEntries = new Array[0]; // array for sending to API that is always empty
}
