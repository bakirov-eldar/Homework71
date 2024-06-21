using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.Services.Email;

public class EmailOptions
{
    [Required]
    public string ServerAddress { get; set; }
    public int ServerPort { get; set; } = 25;
    public bool EnableSsl = false;
    [Required]
    public string EmailAddress { get; set; }
    [Required]
    public string EmailPassword { get; set; }
    [Required]
    public string FromName { get; set; }
}
