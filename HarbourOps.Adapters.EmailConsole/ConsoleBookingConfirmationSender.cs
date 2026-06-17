using HarbourOps.Application.Abstractions;
using HarbourOps.Domain.Bookings;

namespace HarbourOps.Adapters.EmailConsole;

public sealed class ConsoleBookingConfirmationSender : IBookingConfirmationSender
{
    public Task SendAsync(Booking booking, CancellationToken cancellationToken)
    {
        Console.WriteLine("=== Booking confirmation ===");
        Console.WriteLine($"To: {booking.CustomerEmail}");
        Console.WriteLine($"Booking: {booking.Id}");
        Console.WriteLine($"Vessel: {booking.VesselName}");
        Console.WriteLine($"Container: {booking.ContainerNumber}");
        Console.WriteLine($"Total: {booking.Total().Currency} {booking.Total().Amount}");
        Console.WriteLine("============================");

        return Task.CompletedTask;
    }
}
