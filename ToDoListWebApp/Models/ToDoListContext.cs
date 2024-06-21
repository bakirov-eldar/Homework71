using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ToDoListWebApp.Models;

public class ToDoListContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<ToDoTask> ToDoList { get; set; }
    public ToDoListContext(DbContextOptions<ToDoListContext> options) : base(options)
    {

    }
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<DateTimeOffset>().HaveConversion<DateTimeOffsetConverter>();
        configurationBuilder.Properties<DateTimeOffset?>().HaveConversion<NullableDateTimeOffsetConverter>();
    }
}
