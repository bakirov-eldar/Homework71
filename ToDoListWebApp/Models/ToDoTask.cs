using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.Models;

public class ToDoTask
{
    public int Id { get; set; }
    [Display(Name = "Priority")]
    [Required(ErrorMessage = "PriorityRequired")]
    public ToDoTaskPriority Priority { get; set; }
    [Display(Name = "Status")]
    public ToDoTaskStatus Status { get; set; }
    [Display(Name = "Title")]
    [Required(ErrorMessage = "Title")]
    public string Title { get; set; }
    [Display(Name = "Description")]
    [Required(ErrorMessage = "DescriptionRequired")]
    public string Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DoneAt { get; set; }
    public int CreatorId { get; set; }
    public int? PerformerId { get; set; }
    public User? Creator { get; set; }
    public User? Performer { get; set; }
}
