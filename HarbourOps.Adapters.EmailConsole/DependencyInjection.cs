using HarbourOps.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace HarbourOps.Adapters.EmailConsole;

public static class DependencyInjection
{
    public static IServiceCollection AddConsoleEmailAdapter(this IServiceCollection services)
    {
        services.AddScoped<IBookingConfirmationSender, ConsoleBookingConfirmationSender>();
        return services;
    }
}
