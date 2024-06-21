using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.ViewModels;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Текущий пароль")]
    public string OldPassword { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Пароль должен быть не менее {2} и не более {1} символов.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Новый пароль")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Подтверждение нового пароля")]
    [Compare("NewPassword", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
    public string ConfirmPassword { get; set; }
}
