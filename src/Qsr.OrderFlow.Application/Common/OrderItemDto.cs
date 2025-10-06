namespace Qsr.OrderFlow.Application.Common;

public sealed record OrderItemDto(Guid ProductId, int Qty, decimal UnitPrice);
