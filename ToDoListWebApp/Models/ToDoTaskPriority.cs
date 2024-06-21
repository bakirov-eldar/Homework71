using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.Models;

public enum ToDoTaskPriority
{
    [Display(Name = "High")]
    High,
    [Display(Name = "Medium")]
    Medium,
    [Display(Name = "Low")]
    Low
}

public static class ToDoTaskPriorityExtensions
{
    public static string GetCssClass(this ToDoTaskPriority status)
    {
        return status switch
        {
            ToDoTaskPriority.Medium => "priority-medium",
            ToDoTaskPriority.High => "priority-high",
            _ => "priority-low"
        };
    }
}