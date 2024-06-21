using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.Models
{
    public enum ToDoTaskType
    {
        [Display(Name = "All")]
        All,
        [Display(Name = "CreatedByMe")]
        CreatedByMe,
        [Display(Name = "TakenByMe")]
        TakenByMe,
        [Display(Name = "Free")]
        Free
    }
}
