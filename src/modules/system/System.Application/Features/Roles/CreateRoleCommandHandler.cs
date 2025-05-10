using Common.Application.CQRS;
using Common.Application.Interfaces;
using Common.Domain.Results;
using FluentValidation;
using System.Domain.Models;

namespace System.Application.Features.Roles;

public class CreateRoleCommandHandler(IGenericRepository<RoleM> Repository)
    : ICommandHandler<CreateRoleCommand, Result>
{
    public async Task<Result> Handle(
        CreateRoleCommand command,
        CancellationToken cancellation = default)
    {
        var obj = RoleM.Create(command.RoleName);

        await Repository.AddAsync(obj);
        await Repository.SaveChangesAsync(cancellation);

        return Result.Success();
    }
}

public sealed record CreateRoleCommand(string RoleName);

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(_ => _.RoleName).NotEmpty();
    }
}