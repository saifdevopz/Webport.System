namespace System.Domain.Events;

public sealed class UserCreatedDomainEvent(int userId) : DomainEvent
{
    public int UserId { get; init; } = userId;
}