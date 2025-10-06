using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Qsr.OrderFlow.Domain.Events;
using Qsr.OrderFlow.Infrastructure.Persistence;
using System.Text.Json;

namespace Qsr.OrderFlow.Functions;

public class ReceiptGenerator(AppDbContext db, BlobServiceClient blob, ILogger<ReceiptGenerator> log)
{
    private readonly AppDbContext _db = db;
    private readonly BlobServiceClient _blob = blob;
    private readonly ILogger<ReceiptGenerator> _log = log;

    [Function("receipt-generator")]
    public async Task Run(
        [ServiceBusTrigger("orders-topic", "receipt", Connection = "ServiceBus")] string msg)
    {
        _log.LogInformation("receipt-generator triggered: {Msg}", msg);

        var evt = JsonSerializer.Deserialize<PaymentConfirmed>(msg);
        if (evt is null) { _log.LogWarning("Message deserialization failed"); return; }

        var order = await _db.Orders.FindAsync(evt.OrderId);
        if (order is null) { _log.LogWarning("Order {OrderId} not found", evt.OrderId); return; }

        order.MarkPaid();

        await _db.SaveChangesAsync();

        var container = _blob.GetBlobContainerClient("receipts");
        await container.CreateIfNotExistsAsync();
        await container.GetBlobClient($"{order.Id}.txt")
            .UploadAsync(BinaryData.FromString($"Order {order.Id} Total={order.Total}"), overwrite: true);

        _log.LogInformation("Receipt generated for {OrderId}", order.Id);
    }
}