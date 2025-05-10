using Common.Application.CQRS;
using Common.Application.Interfaces;
using Common.Domain.Errors;
using Common.Domain.Results;
using FluentValidation;
using System.Domain.Models;

namespace System.Application.Features.Roles;

public class DeleteRoleCommandHandler(IGenericRepository<RoleM> Repository)
    : ICommandHandler<DeleteRoleCommand, Result>
{
    public async Task<Result> Handle(
        DeleteRoleCommand command,
        CancellationToken cancellation = default)
    {
        var obj = await Repository.FindOneAsync(_ => _.RoleId == command.RoleId, cancellation);

        if (obj is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Role not found."));
        }

        Repository.Delete(obj);
        await Repository.SaveChangesAsync(cancellation);

        return Result.Success();
    }
}

public sealed record DeleteRoleCommand(int RoleId);

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(_ => _.RoleId).NotEmpty();
    }
}