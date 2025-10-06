using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Qsr.OrderFlow.Infrastructure.Persistence;

namespace Qsr.OrderFlow.Worker;

public sealed class Worker(IServiceProvider sp, ILogger<Worker> log, ServiceBusClient sb) : BackgroundService
{
    private readonly IServiceProvider _sp = sp;
    private readonly ILogger<Worker> _log = log;
    private readonly ServiceBusClient _sb = sb;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var sender = _sb.CreateSender("orders-topic");

            var outbox = await db.Outbox
                .Where(o => o.ProcessedOnUtc == null && o.RetryCount < 5)
                .OrderBy(o => o.OccurredOnUtc)
                .Take(50)
                .ToListAsync(stoppingToken);

            if (outbox.Count == 0)
            {
                await Task.Delay(2000, stoppingToken);
                continue;
            }

            foreach (var m in outbox)
            {
                try
                {
                    var sbMsg = new ServiceBusMessage(m.Payload)
                    {
                        Subject = m.Type,
                        CorrelationId = m.Id.ToString()
                    };
                    await sender.SendMessageAsync(sbMsg, stoppingToken);
                    m.ProcessedOnUtc = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    m.RetryCount++;
                    m.Error = ex.Message;
                    _log.LogWarning(ex, "Outbox send failed: {MessageId}", m.Id);
                }
            }

            await db.SaveChangesAsync(stoppingToken);
        }
    }
}
