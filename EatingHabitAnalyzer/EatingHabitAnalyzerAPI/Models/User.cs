using System;
using System.Collections.Generic;

namespace EatingHabitAnalyzerAPI.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public int HeightInInches { get; set; }

    public decimal WeightInPounds { get; set; }

    public DateTime DateOfBirth { get; set; }

    public decimal GoalWeight { get; set; }

    public int GoalDailyCalories { get; set; }

    public string Email { get; set; } = null!;

    public string Pin { get; set; } = null!;

    public virtual ICollection<DiaryEntry> DiaryEntries { get; set; } = new List<DiaryEntry>();
}
