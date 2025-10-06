namespace Qsr.OrderFlow.Application.Orders;

public sealed record OrderItemDto(Guid ProductId, int Qty, decimal UnitPrice);
