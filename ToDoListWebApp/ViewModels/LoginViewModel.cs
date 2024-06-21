using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ToDoListWebApp.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "EmailRequired")]
    [Display(Name = "Email")]
    public string Email { get; set; }
    [Required(ErrorMessage = "PasswordRequired")]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Display(Name = "RememberMe")]
    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}
