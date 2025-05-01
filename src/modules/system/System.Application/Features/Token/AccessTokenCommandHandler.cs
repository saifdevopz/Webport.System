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

        var tokenResult = await tokenService.AccessToken(command.Request);

        if (tokenResult.IsFailure)
        {
            return Result.Failure<AccessTokenResult>(tokenResult.Error!);
        }

        return Result.Success(new AccessTokenResult(tokenResult.Value));
    }
}

public sealed record AccessTokenCommand(AccessTokenRequest Request);

public sealed record AccessTokenResult(TokenResponse Response);

public class AccessTokenCommandValidator : AbstractValidator<AccessTokenCommand>
{
    public AccessTokenCommandValidator()
    {
        RuleFor(_ => _.Request.Email).NotNull().NotEmpty();
        RuleFor(_ => _.Request.Password).NotNull().NotEmpty();
    }
}