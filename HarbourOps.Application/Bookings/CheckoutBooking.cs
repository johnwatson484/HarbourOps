using HarbourOps.Application.Abstractions;

namespace HarbourOps.Application.Bookings;

public sealed class CheckoutBookingHandler(
    IBookingRepository bookings,
    IPaymentGateway paymentGateway,
    IBookingConfirmationSender confirmationSender)
{
    public async Task<BookingDetailsDto?> HandleAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await bookings.GetByIdAsync(bookingId, cancellationToken);
        if (booking is null)
        {
            return null;
        }

        var payment = await paymentGateway.ChargeAsync(
            booking.CustomerEmail,
            booking.Total(),
            $"HarbourOps booking {booking.Id}",
            cancellationToken);

        if (!payment.Succeeded)
        {
            throw new InvalidOperationException(payment.FailureReason ?? "Payment failed.");
        }

        booking.MarkPaid(payment.Reference!);

        await bookings.SaveChangesAsync(cancellationToken);

        await confirmationSender.SendAsync(booking, cancellationToken);

        return booking.ToDetailsDto();
    }
}
