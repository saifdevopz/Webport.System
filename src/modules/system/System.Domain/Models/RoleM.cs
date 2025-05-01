namespace System.Domain.Models;

public sealed class RoleM : AggregateRoot
{
    public int RoleId { get; }
    public required string RoleName { get; set; }
    public required string NormalizedRoleName { get; set; }

    public static RoleM Create(string roleName)
    {
        RoleM obj = new()
        {
            RoleName = roleName,
            NormalizedRoleName = roleName.ToUpperInvariant()
        };

        return obj;
    }
}