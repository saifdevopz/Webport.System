using System.Domain.Events;

namespace System.Domain.Models;

public sealed class UserM : AggregateRoot
{
    public int UserId { get; set; }
    public int TenantId { get; set; }
    public int RoleId { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiration { get; set; }
    public TenantM? Tenant { get; set; }
    public RoleM? Role { get; set; }

    public static UserM Create(
        int tenantId,
        int roleId,
        string fullName,
        string email,
        string password)
    {
        GeneralExtensions.CreatePasswordHash(
            password,
            out byte[] passwordHash,
            out byte[] passwordSalt);

        UserM obj = new()
        {
            TenantId = tenantId,
            RoleId = roleId,
            FullName = fullName,
            Email = email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        obj.AddDomainEvent(new UserCreatedDomainEvent(obj.UserId));

        return obj;
    }
}

