using MediatR;

namespace Qsr.OrderFlow.Application.Orders;

public sealed record CreateOrder(Guid CustomerId, IReadOnlyList<OrderItemDto> Items) : IRequest<Guid>;
