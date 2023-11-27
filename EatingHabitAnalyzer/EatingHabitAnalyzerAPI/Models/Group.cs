using System.ComponentModel.DataAnnotations.Schema;

namespace EatingHabitAnalyzerAPI.Models;

public class Group
{
    public int GroupID { get; set; }

    public int OwnerID { get; set; }

    public string Name { get; set; }

    [NotMapped]
    public List<Goal> Goals { get; } = new List<Goal>();
}
