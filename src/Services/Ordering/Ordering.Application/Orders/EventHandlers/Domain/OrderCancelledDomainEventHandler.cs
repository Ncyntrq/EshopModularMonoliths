using MassTransit;
using BuildingBlocks.Messaging.Events;

namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCancelledDomainEventHandler
    (IPublishEndpoint publishEndpoint, ILogger<OrderCancelledDomainEventHandler> logger)
    : INotificationHandler<OrderCancelledDomainEvent>
{
    public async Task Handle(OrderCancelledDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        var integrationEvent = new OrderCancelledEvent
        {
            OrderId = domainEvent.order.Id.Value
        };

        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
