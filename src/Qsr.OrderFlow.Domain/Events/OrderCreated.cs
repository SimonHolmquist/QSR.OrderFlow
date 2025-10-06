namespace Qsr.OrderFlow.Domain.Events;

public sealed record OrderCreated(Guid OrderId, decimal Total);
