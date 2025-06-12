using FluentValidation;
using System.Domain.Entities.Roles;

namespace System.Application.Features.Roles;

public class UpdateRoleCommandHandler(IGenericRepository<RoleM> Repository)
    : ICommandHandler<UpdateRoleCommand>
{
    public async Task<Result> Handle(
        UpdateRoleCommand command,
        CancellationToken cancellationToken)
    {
        var obj = await Repository.FindOneAsync(_ => _.RoleId == command.RoleId, cancellationToken);

        if (obj == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Role not found."));
        }

        obj.RoleName = command.RoleName;
        obj.NormalizedRoleName = command.RoleName.ToUpperInvariant();

        Repository.Update(obj);
        await Repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record UpdateRoleCommand(int RoleId, string RoleName) : ICommand;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(_ => _.RoleId).NotEmpty();
        RuleFor(_ => _.RoleName).NotEmpty();
    }
}