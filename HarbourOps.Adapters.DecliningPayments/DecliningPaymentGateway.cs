using HarbourOps.Application.Abstractions;
using HarbourOps.Domain;

namespace HarbourOps.Adapters.DecliningPayments;

public sealed class DecliningPaymentGateway : IPaymentGateway
{
    public Task<PaymentResult> ChargeAsync(
        string customerEmail,
        Money amount,
        string description,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(PaymentResult.Failure("Card declined by simulated provider."));
    }
}
