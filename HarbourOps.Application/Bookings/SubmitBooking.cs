using HarbourOps.Application.Abstractions;

namespace HarbourOps.Application.Bookings;

public sealed class SubmitBookingHandler(IBookingRepository bookings)
{
    public async Task<BookingDetailsDto?> HandleAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await bookings.GetByIdAsync(bookingId, cancellationToken);
        if (booking is null) 
        {
            return null;
        }

        booking.Submit();

        await bookings.SaveChangesAsync(cancellationToken);

        return booking.ToDetailsDto();
    }
}
