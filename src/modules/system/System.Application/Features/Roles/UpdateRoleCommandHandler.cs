using Common.Application.CQRS;
using Common.Application.Interfaces;
using Common.Domain.Errors;
using Common.Domain.Results;
using FluentValidation;
using System.Domain.Models;

namespace System.Application.Features.Roles;

public class UpdateRoleCommandHandler(IGenericRepository<RoleM> Repository)
    : ICommandHandler<UpdateRoleCommand, Result>
{
    public async Task<Result> Handle(
        UpdateRoleCommand command,
        CancellationToken cancellation = default)
    {
        var obj = await Repository.FindOneAsync(_ => _.RoleId == command.RoleId, cancellation);

        if (obj == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Role not found."));
        }

        obj.RoleName = command.RoleName;
        obj.NormalizedRoleName = command.RoleName.ToUpperInvariant();

        Repository.Update(obj);
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
        RuleFor(_ => _.RoleId).NotEmpty();
        RuleFor(_ => _.RoleName).NotEmpty();
    }
}