using HarbourOps.Application.Bookings;
using HarbourOps.Domain;
using HarbourOps.Domain.Bookings;

namespace HarbourOps.Tests;

public sealed class ApplicationUseCaseTests
{
    [Fact]
    public async Task Can_create_booking_and_add_service()
    {
        var service = new ServiceItem(
            Guid.NewGuid(),
            "REEFER-MON",
            "Reefer Monitoring",
            "Daily reefer checks.",
            Money.Gbp(125));

        var bookings = new InMemoryBookingRepository();
        var services = new InMemoryServiceCatalogRepository([service]);

        var create = new CreateBookingHandler(bookings);
        var addService = new AddServiceToBookingHandler(bookings, services);

        var booking = await create.HandleAsync(
            new CreateBookingCommand(
                "customer@example.com",
                "MV Northern Star",
                "MSCU1234567",
                DateOnly.FromDateTime(DateTime.Today.AddDays(7))),
            CancellationToken.None);

        var updated = await addService.HandleAsync(
            new AddServiceToBookingCommand(booking.Id, service.Id, 2),
            CancellationToken.None);

        Assert.NotNull(updated);
        Assert.Equal(250m, updated.Total);
        Assert.Single(updated.Lines);
    }
}
