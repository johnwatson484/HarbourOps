using HarbourOps.Domain;

namespace HarbourOps.Domain.Bookings;

public sealed class Booking
{
    private readonly List<BookingLine> _lines = [];

    private Booking() { }

    public Booking(Guid id, string customerEmail, string vesselName, string containerNumber, DateOnly requestedDate)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        if (string.IsNullOrWhiteSpace(customerEmail))
        {
            throw new ArgumentException("Customer email is required.", nameof(customerEmail));
        }
        if (string.IsNullOrWhiteSpace(vesselName))
        {
            throw new ArgumentException("Vessel name is required.", nameof(vesselName));
        }
        if (string.IsNullOrWhiteSpace(containerNumber))
        {
            throw new ArgumentException("Container number is required.", nameof(containerNumber));
        }

        Id = id;
        CustomerEmail = customerEmail;
        VesselName = vesselName;
        ContainerNumber = containerNumber;
        RequestedDate = requestedDate;
        Status = BookingStatus.Draft;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public string CustomerEmail { get; private set; } = "";
    public string VesselName { get; private set; } = "";
    public string ContainerNumber { get; private set; } = "";
    public DateOnly RequestedDate { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public string? PaymentReference { get; private set; }

    public IReadOnlyCollection<BookingLine> Lines => _lines.AsReadOnly();

    public void AddService(ServiceItem serviceItem, int quantity)
    {
        EnsureDraft();

        if (!serviceItem.IsActive)
        {
            throw new InvalidOperationException("Cannot book a retired service.");
        }

        var existing = _lines.FirstOrDefault(x => x.ServiceItemId == serviceItem.Id);

        if (existing is not null)
        {
            throw new InvalidOperationException("Service is already on this booking.");
        }

        _lines.Add(new BookingLine(
            serviceItem.Id,
            serviceItem.Name,
            serviceItem.UnitPrice,
            quantity));
    }

    public Money Total()
    {
        return _lines.Aggregate(Money.Zero(), (total, line) => total.Add(line.LineTotal()));
    }

    public void Submit()
    {
        EnsureDraft();

        if (_lines.Count == 0)
        {
            throw new InvalidOperationException("Cannot submit a booking with no services.");
        }

        Status = BookingStatus.Submitted;
    }

    public void MarkPaid(string paymentReference)
    {
        if (Status != BookingStatus.Submitted)
        {
            throw new InvalidOperationException("Only submitted bookings can be paid.");
        }

        if (string.IsNullOrWhiteSpace(paymentReference))
        {
            throw new ArgumentException("Payment reference is required.", nameof(paymentReference));
        }

        PaymentReference = paymentReference;
        Status = BookingStatus.Paid;
    }

    public void Fulfil()
    {
        if (Status != BookingStatus.Paid)
        {
            throw new InvalidOperationException("Only paid bookings can be fulfilled.");
        }

        Status = BookingStatus.Fulfilled;
    }

    public void Cancel()
    {
        if (Status is BookingStatus.Fulfilled)
        {
            throw new InvalidOperationException("Fulfilled bookings cannot be cancelled.");
        }

        Status = BookingStatus.Cancelled;
    }

    private void EnsureDraft()
    {
        if (Status != BookingStatus.Draft)
        {
            throw new InvalidOperationException("Only draft bookings can be changed.");
        }
    }
}
