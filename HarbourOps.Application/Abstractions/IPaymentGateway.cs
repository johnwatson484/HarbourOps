using HarbourOps.Domain;

namespace HarbourOps.Application.Abstractions;

public interface IPaymentGateway
{
    Task<PaymentResult> ChargeAsync(
        string customerEmail,
        Money amount,
        string description,
        CancellationToken cancellationToken);
}

public sealed record PaymentResult(bool Succeeded, string? Reference, string? FailureReason)
{
    public static PaymentResult Success(string reference) => new(true, reference, null);
    public static PaymentResult Failure(string reason) => new(false, null, reason);
}
