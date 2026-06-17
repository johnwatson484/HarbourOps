using HarbourOps.Application.Abstractions;
using HarbourOps.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace HarbourOps.Adapters.EfSqlite.Persistence;

public sealed class EfBookingRepository(HarbourOpsDbContext db) : IBookingRepository
{
    public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await db.Bookings
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(Booking booking, CancellationToken cancellationToken)
    {
        await db.Bookings.AddAsync(booking, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> ListRecentAsync(int count, CancellationToken cancellationToken)
    {
        return await db.Bookings
            .Include(x => x.Lines)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
}
