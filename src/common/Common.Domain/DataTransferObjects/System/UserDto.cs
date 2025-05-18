using System.ComponentModel.DataAnnotations;

namespace Common.Domain.DataTransferObjects.System;

public class UserDto
{
    public int UserId { get; set; }
    public int TenantId { get; set; }
    public int RoleId { get; set; }

    [Required]
    public string? FullName { get; set; }

    [Required]
    public string? Email { get; set; }
}

public record UserWrapper<T>(T User);
public record UsersWrapper<T>(IEnumerable<T> Users);