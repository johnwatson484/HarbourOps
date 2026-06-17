namespace HarbourOps.Domain;

public sealed record Money(decimal Amount, string Currency)
{
    public static Money Gbp(decimal amount) => new(amount, "GBP");

    public static Money Zero(string currency = "GBP") => new(0, currency);

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add money with different currencies.");

        return this with { Amount = Amount + other.Amount };
    }

    public Money Multiply(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive.");

        return this with { Amount = Amount * quantity };
    }
}
