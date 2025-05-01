namespace System.Domain.DTOs.Users;

public sealed record CreateUserDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int TenantId { get; set; }
}
