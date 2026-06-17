using HarbourOps.Domain;

namespace HarbourOps.Domain.Bookings;

public sealed class BookingLine
{
    private BookingLine() { }

    public BookingLine(Guid serviceItemId, string serviceName, Money unitPrice, int quantity)
    {
        if (serviceItemId == Guid.Empty)
        {
            throw new ArgumentException("Service item id cannot be empty.", nameof(serviceItemId));
        }

        if (string.IsNullOrWhiteSpace(serviceName))
        {
            throw new ArgumentException("Service name is required.", nameof(serviceName));
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive.");
        }

        ServiceItemId = serviceItemId;
        ServiceName = serviceName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ServiceItemId { get; private set; }
    public string ServiceName { get; private set; } = "";
    public Money UnitPrice { get; private set; } = Money.Zero();
    public int Quantity { get; private set; }

    public Money LineTotal() => UnitPrice.Multiply(Quantity);
}
