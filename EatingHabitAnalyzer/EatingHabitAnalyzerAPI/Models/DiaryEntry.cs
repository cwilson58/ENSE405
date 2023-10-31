using System;
using System.Collections.Generic;

namespace EatingHabitAnalyzerAPI.Models;

public partial class DiaryEntry
{
    public int EntryId { get; set; }

    public int? UserId { get; set; }

    public DateTime EntryDateTime { get; set; }

    public bool IsComplete { get; set; }
}
