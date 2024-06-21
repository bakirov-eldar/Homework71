using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ToDoListWebApp.Models;

namespace ToDoListWebApp.ViewModels;

public class IndexViewModel
{
    public List<ToDoTask> ToDoList { get; set; }
    [Display(Name = "Title")]
    public string? Title { get; set; }
    [Display(Name = "CreationDateAfter")]
    public DateTimeOffset? CreationDateAfter { get; set; }
    [Display(Name = "CreationDateBefore")]
    public DateTimeOffset? CreationDateBefore { get; set; }
    [Display(Name = "Description")]
    public string? Description { get; set; }
    [Display(Name = "Status")]
    public ToDoTaskStatus? Status { get; set; }
    [Display(Name = "Priority")]
    public ToDoTaskPriority? Priority { get; set; }
    [Display(Name = "TaskType")]
    public ToDoTaskType TaskType { get; set; }
}
