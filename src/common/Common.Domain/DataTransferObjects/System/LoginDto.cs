﻿using System.ComponentModel.DataAnnotations;

namespace Common.Domain.DataTransferObjects.System;

public class LoginDto
{
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public record RefreshTokenRequest(string Token, string RefreshToken);
public record TokenResponse(string Token, string RefreshToken);


