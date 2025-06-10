namespace System.Domain.Features.Users;

public sealed class UserCreatedDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; init; } = eventId;
}