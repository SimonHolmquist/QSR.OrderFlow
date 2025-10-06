using Qsr.OrderFlow.Domain.Outbox;

namespace Qsr.OrderFlow.Application.Common;

public interface IOutboxRepository
{
    Task AddAsync(OutboxMessage msg, CancellationToken ct);
}
