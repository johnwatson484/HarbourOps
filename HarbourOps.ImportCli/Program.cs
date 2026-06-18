using HarbourOps.Adapters.EfSqlite;
using HarbourOps.Adapters.EfSqlite.Persistence;
using HarbourOps.Application;
using HarbourOps.Application.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["ConnectionStrings:HarbourOps"] = "Data Source=harbourops.dev.db"
    })
    .Build();

var services = new ServiceCollection();

services
    .AddApplication()
    .AddEfSqliteAdapter(configuration);

var provider = services.BuildServiceProvider();

using var scope = provider.CreateScope();

var db = scope.ServiceProvider.GetRequiredService<HarbourOpsDbContext>();
await db.Database.MigrateAsync();

var createBooking = scope.ServiceProvider.GetRequiredService<CreateBookingHandler>();

var result = await createBooking.HandleAsync(
    new CreateBookingCommand(
        "imported.customer@example.com",
        "MV Batch Importer",
        "IMPU7654321",
        DateOnly.FromDateTime(DateTime.Today.AddDays(10))),
    CancellationToken.None);

Console.WriteLine($"Created imported booking {result.Id}");
