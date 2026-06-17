using HarbourOps.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace HarbourOps.Adapters.FakePayments;

public static class DependencyInjection
{
    public static IServiceCollection AddFakePaymentAdapter(this IServiceCollection services)
    {
        services.AddScoped<IPaymentGateway, FakePaymentGateway>();
        return services;
    }
}
