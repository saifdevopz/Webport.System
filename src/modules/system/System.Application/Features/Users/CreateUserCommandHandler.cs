using Common.Application.CQRS;
using Common.Domain.Results;
using FluentValidation;
using System.Application.Interfaces;
using System.Domain.Models;

namespace System.Application.Features.Users;

public class CreateUserCommandHandler(IRepository<UserM> Repository)
    : ICommandHandler<CreateUserCommand, Result>
{
    public async Task<Result> Handle(
        CreateUserCommand command,
        CancellationToken cancellation = default)
    {
        var Obj = UserM.Create(command.FullName, command.Email, command.Password, command.TenantId, command.RoleId);

        await Repository.AddAsync(Obj);
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
        RuleFor(_ => _.FullName).NotNull().NotEmpty();
        RuleFor(_ => _.Email).NotNull().NotEmpty().EmailAddress();
        RuleFor(_ => _.Password).NotNull().NotEmpty();
        RuleFor(_ => _.TenantId).NotNull().NotEmpty();
    }
}