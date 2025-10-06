namespace Qsr.OrderFlow.Domain.Orders;

public sealed class Order
{
    private Order() { } // EF
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public bool Paid { get; set; }
    private readonly List<OrderItem> _items = [];
    public IReadOnlyList<OrderItem> Items => _items;
    public decimal Total { get; private set; }

    public static Order Create(Guid customerId, IEnumerable<OrderItem> items)
    {
        var o = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            CreatedOnUtc = DateTime.UtcNow
        };
        o._items.AddRange(items);
        o.Total = o._items.Sum(i => i.Qty * i.UnitPrice);
        return o;
    }

    public void MarkPaid() => Paid = true;
}
