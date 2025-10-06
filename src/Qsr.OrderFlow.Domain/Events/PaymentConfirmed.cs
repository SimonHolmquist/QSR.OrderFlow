namespace Qsr.OrderFlow.Domain.Events;

public sealed record PaymentConfirmed(Guid OrderId, DateTime PaidOnUtc);