using Common.Application.CQRS;
using Common.Domain.Results;
using FluentValidation;
using System.Application.Interfaces;
using System.Domain.Models;

namespace System.Application.Features.Tenant;

public class CreateTenantCommandHandler(IRepository<TenantM> Repository)
    : ICommandHandler<CreateTenantCommand, Result>
{
    public async Task<Result> Handle(
        CreateTenantCommand command,
        CancellationToken cancellation = default)
    {
        var obj = TenantM.Create(command.TenantName, command.DatabaseName);

        await Repository.AddAsync(obj);
        await Repository.SaveChangesAsync(cancellation);

        return Result.Success();
    }
}

public sealed record CreateTenantCommand(
    string TenantName,
    string DatabaseName);

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(_ => _.TenantName).NotEmpty();
        RuleFor(_ => _.DatabaseName).NotEmpty();
    }
}