using HarbourOps.Application.Abstractions;
using HarbourOps.Domain.Bookings;

namespace HarbourOps.Application.Bookings;

public sealed record CreateBookingCommand(
    string CustomerEmail,
    string VesselName,
    string ContainerNumber,
    DateOnly RequestedDate);

public sealed class CreateBookingHandler(IBookingRepository bookings)
{
    public async Task<BookingDetailsDto> HandleAsync(
        CreateBookingCommand command,
        CancellationToken cancellationToken)
    {
        var booking = new Booking(
            Guid.NewGuid(),
            command.CustomerEmail,
            command.VesselName,
            command.ContainerNumber,
            command.RequestedDate);

        await bookings.AddAsync(booking, cancellationToken);
        await bookings.SaveChangesAsync(cancellationToken);

        return booking.ToDetailsDto();
    }
}
