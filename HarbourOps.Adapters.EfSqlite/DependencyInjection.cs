using HarbourOps.Adapters.EfSqlite.Persistence;
using HarbourOps.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HarbourOps.Adapters.EfSqlite;

public static class DependencyInjection
{
    public static IServiceCollection AddEfSqliteAdapter(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("HarbourOps")
            ?? "Data Source=harbourops.db";

        services.AddDbContext<HarbourOpsDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IBookingRepository, EfBookingRepository>();
        services.AddScoped<IServiceCatalogRepository, EfServiceCatalogRepository>();

        return services;
    }
}
