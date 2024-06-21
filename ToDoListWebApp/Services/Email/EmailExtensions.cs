using ToDoListWebApp.Services.Email;

namespace ToDoListWebApp;

public static class EmailExtensions
{
    public static IServiceCollection AddEmail(this IServiceCollection services, Action<EmailOptions> configureOptions)
    {
        services.AddOptions<EmailOptions>()
            .BindConfiguration("Email")
            .Configure(configureOptions)
            .ValidateDataAnnotations();
        services.AddSingleton<EmailService>();
        return services;
    }
    public static IServiceCollection AddEmail(this IServiceCollection services)
    {
        services.AddEmail((options) => { });
        return services;
    }
}
