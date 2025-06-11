using FluentValidation;
using System.Domain.Entities.Roles;

namespace System.Application.Features.Roles;

public class DeleteRoleCommandHandler(IGenericRepository<RoleM> Repository)
    : ICommandHandler<DeleteRoleCommand>
{
    public async Task<Result> Handle(
        DeleteRoleCommand command,
        CancellationToken cancellationToken)
    {
        var obj = await Repository.FindOneAsync(_ => _.RoleId == command.RoleId, cancellationToken);

        if (obj is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Role not found."));
        }

        Repository.Delete(obj);
        await Repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record DeleteRoleCommand(int RoleId) : ICommand;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(_ => _.RoleId).NotEmpty();
    }
}