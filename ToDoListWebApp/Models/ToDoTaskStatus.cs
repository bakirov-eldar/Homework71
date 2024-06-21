using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.Models;

public enum ToDoTaskStatus
{
    [Display(Name = "New")]
    New,
    [Display(Name = "Opened")]
    InProgress,
    [Display(Name = "Closed")]
    Done
}
