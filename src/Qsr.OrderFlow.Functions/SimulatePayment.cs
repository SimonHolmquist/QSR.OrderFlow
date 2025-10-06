using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Qsr.OrderFlow.Domain.Events;
using System.Net;
using System.Text.Json;

namespace Qsr.OrderFlow.Functions;

public class SimulatePayment(IConfiguration cfg, ILogger<SimulatePayment> log)
{
    private readonly ServiceBusClient _sb = new(cfg["ServiceBus"]);

    [Function("simulate-payment")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "simulate-payment/{orderId:guid}")]
        HttpRequestData req, Guid orderId)
    {
        var ns = _sb.FullyQualifiedNamespace; // debería ser sb-orderflow-9752.servicebus.windows.net
        log.LogInformation("Sending PaymentConfirmed for {OrderId} to namespace {Ns} topic {Topic}", orderId, ns, "orders-topic");

        var sender = _sb.CreateSender("orders-topic");
        var payload = JsonSerializer.Serialize(new PaymentConfirmed(orderId, DateTime.UtcNow));
        var msg = new ServiceBusMessage(payload) { Subject = nameof(PaymentConfirmed) };

        await sender.SendMessageAsync(msg);
        var res = req.CreateResponse(HttpStatusCode.OK);
        await res.WriteStringAsync("OK");
        return res;
    }
}