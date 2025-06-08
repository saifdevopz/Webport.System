using Common.Domain.Abstractions;

namespace Common.Application.Messaging;

public abstract class DomainEventDispatcher<TDomainEvent> : IDomainEventDispatcher<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public abstract Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default);

    public Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken = default) =>
        Handle((TDomainEvent)domainEvent, cancellationToken);
}