namespace Qsr.OrderFlow.Domain.Orders;

public sealed class OrderItem
{
    private OrderItem() { }
    public Guid ProductId { get; private set; }
    public int Qty { get; private set; }
    public decimal UnitPrice { get; private set; }

    public static OrderItem Create(Guid productId, int qty, decimal unitPrice)
        => new OrderItem { ProductId = productId, Qty = qty, UnitPrice = unitPrice };
}