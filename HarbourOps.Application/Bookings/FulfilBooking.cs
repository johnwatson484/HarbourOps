using HarbourOps.Application.Abstractions;

namespace HarbourOps.Application.Bookings;

public sealed class FulfilBookingHandler(IBookingRepository bookings)
{
    public async Task<BookingDetailsDto?> HandleAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await bookings.GetByIdAsync(bookingId, cancellationToken);
        if (booking is null) 
        {
            return null;
        }

        booking.Fulfil();

        await bookings.SaveChangesAsync(cancellationToken);

        return booking.ToDetailsDto();
    }
}
