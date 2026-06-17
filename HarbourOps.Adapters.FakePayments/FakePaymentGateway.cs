using HarbourOps.Application.Abstractions;
using HarbourOps.Domain;

namespace HarbourOps.Adapters.FakePayments;

public sealed class FakePaymentGateway : IPaymentGateway
{
    public Task<PaymentResult> ChargeAsync(
        string customerEmail,
        Money amount,
        string description,
        CancellationToken cancellationToken)
    {
        if (amount.Amount <= 0)
        {
            return Task.FromResult(PaymentResult.Failure("Amount must be greater than zero."));
        }
        
        var reference = $"FAKE-{DateTimeOffset.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}";
        return Task.FromResult(PaymentResult.Success(reference));
    }
}
