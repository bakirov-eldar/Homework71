using Microsoft.AspNetCore.Identity;

namespace ToDoListWebApp.Models
{
    public class User : IdentityUser<int>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

    }
}
