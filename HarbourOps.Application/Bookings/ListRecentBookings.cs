using HarbourOps.Application.Abstractions;

namespace HarbourOps.Application.Bookings;

public sealed class ListRecentBookingsHandler(IBookingRepository bookings)
{
    public async Task<IReadOnlyList<BookingSummaryDto>> HandleAsync(
        int count,
        CancellationToken cancellationToken)
    {
        var results = await bookings.ListRecentAsync(count, cancellationToken);
        return results.Select(x => x.ToSummaryDto()).ToList();
    }
}
