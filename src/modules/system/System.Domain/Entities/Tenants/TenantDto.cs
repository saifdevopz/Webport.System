using System.ComponentModel.DataAnnotations;

namespace System.Domain.Entities.Tenants;

public sealed record CreateTenantDto
{
    [Required]
    public required string TenantName { get; init; }

    [Required]
    public required string DatabaseName { get; init; }
}

public sealed record TenantDto
{
    public string? TenantName { get; init; }
    public string? DatabaseName { get; init; }
}

public sealed record CollectionDto
{
    public IEnumerable<TenantDto>? Data { get; init; }
}
