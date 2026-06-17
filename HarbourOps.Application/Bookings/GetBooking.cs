using HarbourOps.Application.Abstractions;

namespace HarbourOps.Application.Bookings;

public sealed class GetBookingHandler(IBookingRepository bookings)
{
    public async Task<BookingDetailsDto?> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var booking = await bookings.GetByIdAsync(id, cancellationToken);
        return booking?.ToDetailsDto();
    }
}
