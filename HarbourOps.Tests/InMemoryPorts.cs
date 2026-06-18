using HarbourOps.Application.Abstractions;
using HarbourOps.Domain.Bookings;

namespace HarbourOps.Tests;

internal sealed class InMemoryBookingRepository : IBookingRepository
{
    private readonly List<Booking> _bookings = [];

    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_bookings.FirstOrDefault(x => x.Id == id));
    }

    public Task AddAsync(Booking booking, CancellationToken cancellationToken)
    {
        _bookings.Add(booking);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Booking>> ListRecentAsync(int count, CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyList<Booking>>(
            _bookings.OrderByDescending(x => x.CreatedAtUtc).Take(count).ToList());
    }
}

internal sealed class InMemoryServiceCatalogRepository(IEnumerable<ServiceItem> services)
    : IServiceCatalogRepository
{
    private readonly List<ServiceItem> _services = services.ToList();

    public Task<ServiceItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_services.FirstOrDefault(x => x.Id == id));
    }

    public Task<IReadOnlyList<ServiceItem>> ListActiveAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyList<ServiceItem>>(
            _services.Where(x => x.IsActive).ToList());
    }
}
