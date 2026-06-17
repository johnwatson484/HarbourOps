using HarbourOps.Application.Abstractions;

namespace HarbourOps.Application.Services;

public sealed class ListServicesHandler(IServiceCatalogRepository services)
{
    public async Task<IReadOnlyList<ServiceItemDto>> HandleAsync(CancellationToken cancellationToken)
    {
        var items = await services.ListActiveAsync(cancellationToken);

        return items.Select(x => new ServiceItemDto(
            x.Id,
            x.Code,
            x.Name,
            x.Description,
            x.UnitPrice.Amount,
            x.UnitPrice.Currency)).ToList();
    }
}
