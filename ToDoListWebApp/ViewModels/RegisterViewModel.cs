using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "FirstnameRequired")]
    [DisplayName("Firstname")]
    public string Firstname { get; set; }
    [Required(ErrorMessage = "LastnameRequired")]
    [DisplayName("Lastname")]
    public string Lastname { get; set; }
    [Required(ErrorMessage = "EmailRequired")]
    [DisplayName("Email")]
    public string Email { get; set; }
    [Required(ErrorMessage = "PasswordRequired")]
    [DisplayName("Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required(ErrorMessage = "ConfirmPasswordRequired")]
    [DisplayName("ConfirmPassword")]
    [Compare(nameof(Password), ErrorMessage = "ConfirmPasswordCompare")]
    public string RepeatPassword { get; set; }
}
