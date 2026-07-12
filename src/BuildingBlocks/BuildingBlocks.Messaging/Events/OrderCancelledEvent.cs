namespace BuildingBlocks.Messaging.Events;

public record OrderCancelledEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}
