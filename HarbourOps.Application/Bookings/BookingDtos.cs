using HarbourOps.Domain.Bookings;

namespace HarbourOps.Application.Bookings;

public sealed record BookingSummaryDto(
    Guid Id,
    string CustomerEmail,
    string VesselName,
    string ContainerNumber,
    DateOnly RequestedDate,
    BookingStatus Status,
    decimal Total,
    string Currency);

public sealed record BookingDetailsDto(
    Guid Id,
    string CustomerEmail,
    string VesselName,
    string ContainerNumber,
    DateOnly RequestedDate,
    BookingStatus Status,
    decimal Total,
    string Currency,
    IReadOnlyList<BookingLineDto> Lines,
    string? PaymentReference);

public sealed record BookingLineDto(
    Guid ServiceItemId,
    string ServiceName,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal);
