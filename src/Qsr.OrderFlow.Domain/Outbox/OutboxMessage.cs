namespace Qsr.OrderFlow.Domain.Outbox;

public sealed class OutboxMessage
{
    private OutboxMessage() { }
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; private set; }
    public string Type { get; private set; } = "";
    public string Payload { get; private set; } = "";
    public int RetryCount { get; set; }
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }

    public static OutboxMessage From(string type, string payload) =>
        new()
        { OccurredOnUtc = DateTime.UtcNow, Type = type, Payload = payload };
}
