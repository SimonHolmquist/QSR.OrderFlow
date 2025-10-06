using Qsr.OrderFlow.Application.Common;
using Qsr.OrderFlow.Domain.Orders;

namespace Qsr.OrderFlow.Infrastructure.Persistence;

public sealed class EfOrderRepository(AppDbContext db) : IOrderRepository
{
    private readonly AppDbContext _db = db;

    public Task AddAsync(Order o, CancellationToken ct) => _db.Orders.AddAsync(o, ct).AsTask();
    public Task<Order?> GetByIdAsync(Guid id, CancellationToken ct) => _db.Orders.FindAsync([id], ct).AsTask();
    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
