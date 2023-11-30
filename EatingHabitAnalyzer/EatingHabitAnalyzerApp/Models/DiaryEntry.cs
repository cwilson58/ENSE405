using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatingHabitAnalyzerApp.Models;

public class DiaryEntry
{
    public int EntryId { get; set; }

    public DateTime Date { get; set; }

    public int UserId { get; set; }

    public bool IsCompleted { get; set; }

    public ExerciseLog Exercise { get; set; }

    public List<Meal> Meals { get; set; }
}
