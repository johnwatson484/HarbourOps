using HarbourOps.Domain;

namespace HarbourOps.Domain.Bookings;

public sealed class ServiceItem
{
    private ServiceItem() { }

    public ServiceItem(
        Guid id,
        string code,
        string name,
        string description,
        Money unitPrice,
        bool isActive = true)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Code is required.", nameof(code));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }
        if (unitPrice.Amount < 0)
        {
            throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));
        }

        Id = id;
        Code = code;
        Name = name;
        Description = description;
        UnitPrice = unitPrice;
        IsActive = isActive;
    }

    public Guid Id { get; private set; }
    public string Code { get; private set; } = "";
    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";
    public Money UnitPrice { get; private set; } = Money.Zero();
    public bool IsActive { get; private set; }

    public void Retire() => IsActive = false;
}
