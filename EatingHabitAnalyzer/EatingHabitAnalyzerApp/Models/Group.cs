using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatingHabitAnalyzerApp.Models;

public class Group
{
    public int GroupId { get; set; }

    public int OwnerId { get; set; }

    public string Name { get; set; }

    public List<Goal> Goals { set; get; }
}
