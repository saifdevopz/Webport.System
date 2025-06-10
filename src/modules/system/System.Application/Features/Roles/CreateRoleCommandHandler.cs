using FluentValidation;
using System.Domain.Models;

namespace System.Application.Features.Roles;

public class CreateRoleCommandHandler(IGenericRepository<RoleM> Repository)
    : ICommandHandler<CreateRoleCommand>
{
    public async Task<Result> Handle(
        CreateRoleCommand command,
        CancellationToken cancellationToken)
    {
        var obj = RoleM.Create(command.RoleName);

        await Repository.AddAsync(obj);
        await Repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record CreateRoleCommand(string RoleName) : ICommand;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(_ => _.RoleName).NotEmpty();
    }
}