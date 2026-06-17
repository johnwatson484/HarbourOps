using HarbourOps.Application.Abstractions;
using HarbourOps.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace HarbourOps.Adapters.EfSqlite.Persistence;

public sealed class EfServiceCatalogRepository(HarbourOpsDbContext db) : IServiceCatalogRepository
{
    public async Task<ServiceItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await db.ServiceItems.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ServiceItem>> ListActiveAsync(CancellationToken cancellationToken)
    {
        return await db.ServiceItems
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }
}
