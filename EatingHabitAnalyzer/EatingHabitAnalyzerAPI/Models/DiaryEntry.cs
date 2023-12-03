using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatingHabitAnalyzerAPI.Models;

public partial class DiaryEntry
{
    public int EntryId { get; set; }

    public int? UserId { get; set; }

    public DateTime EntryDateTime { get; set; }

    public bool IsComplete { get; set; }

    [NotMapped]
    public List<Meal> Meals { get; } = new List<Meal>();

    [NotMapped]
    public ExerciseLog? Exercise { get; set; }

    [NotMapped]
    public FeelingSurvey? FeelingsSurvey { get; set; }
}
