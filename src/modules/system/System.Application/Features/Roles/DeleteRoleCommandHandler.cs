using Common.Application.CQRS;
using Common.Domain.Errors;
using Common.Domain.Results;
using FluentValidation;
using System.Application.Interfaces;
using System.Domain.Models;

namespace System.Application.Features.Roles;

public class DeleteRoleCommandHandler(IRepository<RoleM> Repository)
    : ICommandHandler<DeleteRoleCommand, Result>
{
    public async Task<Result> Handle(
        DeleteRoleCommand command,
        CancellationToken cancellation = default)
    {
        var Obj = await Repository.FindOneAsync(_ => _.RoleId == command.RoleId, cancellation);

        if (Obj is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Role not found."));
        }

        Repository.Delete(Obj);
        await Repository.SaveChangesAsync(cancellation);

        return Result.Success();
    }
}

public sealed record DeleteRoleCommand(int RoleId);

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(_ => _.RoleId).NotNull().NotEmpty();
    }
}