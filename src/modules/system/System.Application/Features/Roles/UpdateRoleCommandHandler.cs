using Common.Application.CQRS;
using Common.Domain.Errors;
using Common.Domain.Results;
using FluentValidation;
using System.Application.Interfaces;
using System.Domain.Models;

namespace System.Application.Features.Roles;

public class UpdateRoleCommandHandler(IRepository<RoleM> Repository)
    : ICommandHandler<UpdateRoleCommand, Result>
{
    public async Task<Result> Handle(
        UpdateRoleCommand command,
        CancellationToken cancellation = default)
    {
        var Obj = await Repository.FindOneAsync(_ => _.RoleId == command.RoleId, cancellation);

        if (Obj == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Role not found."));
        }

        Obj.RoleName = command.RoleName;

        Repository.Update(Obj);
        await Repository.SaveChangesAsync(cancellation);

        return Result.Success();
    }
}

public sealed record UpdateRoleCommand(
    int RoleId,
    string RoleName);

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(_ => _.RoleId).NotNull().NotEmpty();
        RuleFor(_ => _.RoleName).NotNull().NotEmpty();
    }
}