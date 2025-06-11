namespace System.Domain.Entities.Users;

public sealed class UserCreatedDomainEvent(int userId) : DomainEvent
{
    public int UserId { get; init; } = userId;
}