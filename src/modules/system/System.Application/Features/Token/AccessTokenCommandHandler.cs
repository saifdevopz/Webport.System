using Common.Application.CQRS;
using Common.Domain.Results;
using FluentValidation;
using System.Application.Dtos;
using System.Application.Interfaces;

namespace System.Application.Features.Token;

public class AccessTokenCommandHandler(ITokenService tokenService)
    : ICommandHandler<AccessTokenCommand, Result<AccessTokenResult>>
{
    public async Task<Result<AccessTokenResult>> Handle(
        AccessTokenCommand command,
        CancellationToken cancellation = default)
    {

        var tokenResult = await tokenService.AccessToken(new AccessTokenRequest(command.Email, command.Password));

        if (tokenResult.IsFailure)
        {
            return Result.Failure<AccessTokenResult>(tokenResult.Error!);
        }

        return Result.Success(new AccessTokenResult(
            tokenResult.Data.Token,
            tokenResult.Data.RefreshToken));
    }
}

public sealed record AccessTokenCommand(string Email, string Password);

public sealed record AccessTokenResult(string Token, string RefreshToken);

public class AccessTokenCommandValidator : AbstractValidator<AccessTokenCommand>
{
    public AccessTokenCommandValidator()
    {
        RuleFor(_ => _.Email).NotEmpty();
        RuleFor(_ => _.Password).NotEmpty();
    }
}