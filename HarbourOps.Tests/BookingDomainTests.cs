using HarbourOps.Domain;
using HarbourOps.Domain.Bookings;

namespace HarbourOps.Tests;

public sealed class BookingDomainTests
{
    [Fact]
    public void New_booking_starts_as_draft()
    {
        var booking = NewBooking();

        Assert.Equal(BookingStatus.Draft, booking.Status);
    }

    [Fact]
    public void Cannot_submit_empty_booking()
    {
        var booking = NewBooking();

        var ex = Assert.Throws<InvalidOperationException>(() => booking.Submit());

        Assert.Contains("no services", ex.Message);
    }

    [Fact]
    public void Booking_total_is_sum_of_lines()
    {
        var booking = NewBooking();

        booking.AddService(NewService("REEFER", 125), 2);
        booking.AddService(NewService("VGM", 85), 1);

        var total = booking.Total();

        Assert.Equal(335m, total.Amount);
        Assert.Equal("GBP", total.Currency);
    }

    [Fact]
    public void Cannot_add_service_after_submit()
    {
        var booking = NewBooking();

        booking.AddService(NewService("REEFER", 125), 1);
        booking.Submit();

        Assert.Throws<InvalidOperationException>(() =>
            booking.AddService(NewService("VGM", 85), 1));
    }

    private static Booking NewBooking()
    {
        return new Booking(
            Guid.NewGuid(),
            "customer@example.com",
            "MV Test",
            "TEST1234567",
            DateOnly.FromDateTime(DateTime.Today.AddDays(7)));
    }

    private static ServiceItem NewService(string code, decimal price)
    {
        return new ServiceItem(
            Guid.NewGuid(),
            code,
            code,
            $"{code} service",
            Money.Gbp(price));
    }
}
