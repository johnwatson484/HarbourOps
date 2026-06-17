using HarbourOps.Domain.Bookings;

namespace HarbourOps.Application.Bookings;

internal static class BookingMapping
{
    public static BookingSummaryDto ToSummaryDto(this Booking booking)
    {
        var total = booking.Total();

        return new BookingSummaryDto(
            booking.Id,
            booking.CustomerEmail,
            booking.VesselName,
            booking.ContainerNumber,
            booking.RequestedDate,
            booking.Status,
            total.Amount,
            total.Currency);
    }

    public static BookingDetailsDto ToDetailsDto(this Booking booking)
    {
        var total = booking.Total();

        return new BookingDetailsDto(
            booking.Id,
            booking.CustomerEmail,
            booking.VesselName,
            booking.ContainerNumber,
            booking.RequestedDate,
            booking.Status,
            total.Amount,
            total.Currency,
            booking.Lines.Select(line => new BookingLineDto(
                line.ServiceItemId,
                line.ServiceName,
                line.UnitPrice.Amount,
                line.Quantity,
                line.LineTotal().Amount)).ToList(),
            booking.PaymentReference);
    }
}
