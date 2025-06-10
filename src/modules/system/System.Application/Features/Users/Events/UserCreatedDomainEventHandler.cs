using System.Domain.Events;

namespace System.Application.Features.Users.Events;


internal sealed class UserCreatedDomainEventHandler()
    : DomainEventDispatcher<UserCreatedDomainEvent>
{
    public override async Task Handle(
        UserCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
#pragma warning disable S1481 // Unused local variables should be removed
        var ghj = domainEvent.UserId;
#pragma warning restore S1481 // Unused local variables should be removed
        await Task.Delay(500, cancellationToken);
    }
}