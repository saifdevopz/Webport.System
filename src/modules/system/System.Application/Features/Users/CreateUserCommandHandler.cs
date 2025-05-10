using Common.Application.CQRS;
using Common.Application.Interfaces;
using Common.Domain.Results;
using FluentValidation;
using System.Domain.Models;

namespace System.Application.Features.Users;

public class CreateUserCommandHandler(IGenericRepository<UserM> Repository)
    : ICommandHandler<CreateUserCommand, Result>
{
    public async Task<Result> Handle(
        CreateUserCommand command,
        CancellationToken cancellation = default)
    {
        var obj = UserM.Create(command.TenantId, command.RoleId, command.FullName, command.Email, command.Password);

        await Repository.AddAsync(obj);
        await Repository.SaveChangesAsync(cancellation);

        return Result.Success();
    }
}

public sealed record CreateUserCommand(
    string FullName,
    string Email,
    string Password,
    int TenantId,
    int RoleId);

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(_ => _.FullName).NotEmpty();
        RuleFor(_ => _.Email).NotEmpty().EmailAddress();
        RuleFor(_ => _.Password).NotEmpty();
        RuleFor(_ => _.TenantId).NotEmpty();
    }
}