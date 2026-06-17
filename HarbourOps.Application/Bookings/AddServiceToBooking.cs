using HarbourOps.Application.Abstractions;

namespace HarbourOps.Application.Bookings;

public sealed record AddServiceToBookingCommand(Guid BookingId, Guid ServiceItemId, int Quantity);

public sealed class AddServiceToBookingHandler(
    IBookingRepository bookings,
    IServiceCatalogRepository services)
{
    public async Task<BookingDetailsDto?> HandleAsync(
        AddServiceToBookingCommand command,
        CancellationToken cancellationToken)
    {
        var booking = await bookings.GetByIdAsync(command.BookingId, cancellationToken);
        if (booking is null) 
        {
            return null;
        }

        var service = await services.GetByIdAsync(command.ServiceItemId, cancellationToken);
        if (service is null) 
        {
            return null;
        }

        booking.AddService(service, command.Quantity);

        await bookings.SaveChangesAsync(cancellationToken);

        return booking.ToDetailsDto();
    }
}
