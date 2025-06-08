using Common.Domain.Abstractions;

namespace Common.Application.Messaging;

public interface IDomainEventDispatcher<in TDomainEvent> : IDomainEventDispatcher
    where TDomainEvent : IDomainEvent
{
    Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
}

public interface IDomainEventDispatcher
{
    Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}