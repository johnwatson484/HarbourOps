using HarbourOps.Domain.Bookings;

namespace HarbourOps.Application.Abstractions;

public interface IBookingConfirmationSender
{
    Task SendAsync(Booking booking, CancellationToken cancellationToken);
}
