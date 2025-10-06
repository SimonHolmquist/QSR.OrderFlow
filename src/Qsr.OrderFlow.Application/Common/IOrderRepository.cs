using Qsr.OrderFlow.Domain.Orders;

namespace Qsr.OrderFlow.Application.Common;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken ct);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}