using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.ViewModels;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "OldPasswordRequired")]
    [DataType(DataType.Password)]
    [Display(Name = "OldPassword")]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = "NewPasswordRequired")]
    [DataType(DataType.Password)]
    [Display(Name = "NewPassword")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "ConfirmPassword")]
    [Compare("NewPassword", ErrorMessage = "RequireCompare")]
    public string ConfirmPassword { get; set; }
}
