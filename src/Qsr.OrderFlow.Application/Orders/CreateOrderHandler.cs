using MediatR;
using Qsr.OrderFlow.Application.Common;
using Qsr.OrderFlow.Domain.Events;
using Qsr.OrderFlow.Domain.Orders;
using Qsr.OrderFlow.Domain.Outbox;
using System.Text.Json;

namespace Qsr.OrderFlow.Application.Orders;

public sealed class CreateOrderHandler(IOrderRepository orders, IOutboxRepository outbox) : IRequestHandler<CreateOrder, Guid>
{
    private readonly IOrderRepository _orders = orders;
    private readonly IOutboxRepository _outbox = outbox;

    public async Task<Guid> Handle(CreateOrder req, CancellationToken ct)
    {
        var items = req.Items.Select(i => OrderItem.Create(i.ProductId, i.Qty, i.UnitPrice));
        var order = Order.Create(req.CustomerId, items);
        await _orders.AddAsync(order, ct);

        var evt = new OrderCreated(order.Id, order.Total);
        var payload = JsonSerializer.Serialize(evt);
        await _outbox.AddAsync(OutboxMessage.From(nameof(OrderCreated), payload), ct);

        await _orders.SaveChangesAsync(ct);
        return order.Id;
    }
}
