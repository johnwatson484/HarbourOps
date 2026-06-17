using HarbourOps.Domain.Bookings;

namespace HarbourOps.Application.Abstractions;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Booking booking, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Booking>> ListRecentAsync(int count, CancellationToken cancellationToken);
}
