namespace HarbourOps.Application.Services;

public sealed record ServiceItemDto(
    Guid Id,
    string Code,
    string Name,
    string Description,
    decimal UnitPrice,
    string Currency);
