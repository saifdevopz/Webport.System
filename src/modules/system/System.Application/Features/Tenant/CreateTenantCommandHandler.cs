using FluentValidation;
using System.Domain.Models;

namespace System.Application.Features.Tenant;

public class CreateTenantCommandHandler(IGenericRepository<TenantM> Repository)
    : ICommandHandler<CreateTenantCommand>
{
    public async Task<Result> Handle(
        CreateTenantCommand command,
        CancellationToken cancellationToken)
    {
        var obj = TenantM.Create(command.TenantName, command.DatabaseName);

        await Repository.AddAsync(obj);
        await Repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record CreateTenantCommand(
    string TenantName,
    string DatabaseName) : ICommand;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(_ => _.TenantName).NotEmpty();
        RuleFor(_ => _.DatabaseName).NotEmpty();
    }
}