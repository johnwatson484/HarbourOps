using HarbourOps.Domain.Bookings;

namespace HarbourOps.Application.Abstractions;

public interface IServiceCatalogRepository
{
    Task<ServiceItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<ServiceItem>> ListActiveAsync(CancellationToken cancellationToken);
}
