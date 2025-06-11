using FluentValidation;
using System.Domain.Entities.Users;

namespace System.Application.Features.Users;

public class CreateUserCommandHandler(IGenericRepository<UserM> Repository)
    : ICommandHandler<CreateUserCommand>
{
    public async Task<Result> Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var obj = UserM.Create(command.TenantId, command.RoleId, command.FullName, command.Email, command.Password);

        await Repository.AddAsync(obj);
        await Repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record CreateUserCommand(
    string FullName,
    string Email,
    string Password,
    int TenantId,
    int RoleId) : ICommand;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(_ => _.FullName).NotEmpty();
        RuleFor(_ => _.Email).NotEmpty().EmailAddress();
        RuleFor(_ => _.Password).NotEmpty();
        RuleFor(_ => _.TenantId).NotEmpty();
    }
}