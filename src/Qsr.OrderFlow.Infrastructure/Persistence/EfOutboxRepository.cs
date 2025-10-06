using Qsr.OrderFlow.Application.Common;
using Qsr.OrderFlow.Domain.Outbox;

namespace Qsr.OrderFlow.Infrastructure.Persistence;

public sealed class EfOutboxRepository(AppDbContext db) : IOutboxRepository
{
    private readonly AppDbContext _db = db;

    public Task AddAsync(OutboxMessage m, CancellationToken ct) => _db.Outbox.AddAsync(m, ct).AsTask();
}