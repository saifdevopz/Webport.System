using Common.Application.CQRS;
using Common.Domain.Results;
using FluentValidation;
using System.Application.Interfaces;
using System.Domain.Models;

namespace System.Application.Features.Roles;

public class CreateRoleCommandHandler(IRepository<RoleM> Repository)
    : ICommandHandler<CreateRoleCommand, Result>
{
    public async Task<Result> Handle(
        CreateRoleCommand command,
        CancellationToken cancellation = default)
    {
        var Obj = RoleM.Create(command.RoleName);

        await Repository.AddAsync(Obj);
        await Repository.SaveChangesAsync(cancellation);

        return Result.Success();
    }
}

public sealed record CreateRoleCommand(string RoleName);

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(_ => _.RoleName).NotNull().NotEmpty();
    }
}