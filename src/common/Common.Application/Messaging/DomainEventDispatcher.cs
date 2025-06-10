﻿using Common.Domain.Abstractions;

namespace Common.Application.Messaging;

public abstract class DomainEventDispatcher<TDomainEvent> : IDomainEventDispatcher<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public abstract Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken);

    public Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken) =>
        Handle((TDomainEvent)domainEvent, cancellationToken);
}