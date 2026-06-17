using HarbourOps.Application.Bookings;
using HarbourOps.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HarbourOps.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateBookingHandler>();
        services.AddScoped<AddServiceToBookingHandler>();
        services.AddScoped<SubmitBookingHandler>();
        services.AddScoped<CheckoutBookingHandler>();
        services.AddScoped<FulfilBookingHandler>();
        services.AddScoped<GetBookingHandler>();
        services.AddScoped<ListRecentBookingsHandler>();
        services.AddScoped<ListServicesHandler>();

        return services;
    }
}
