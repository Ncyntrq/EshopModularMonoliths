namespace Ordering.Domain.Events;

public record OrderCancelledDomainEvent(Order order) : IDomainEvent;
