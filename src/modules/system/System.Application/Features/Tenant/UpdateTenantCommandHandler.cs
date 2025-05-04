using Common.Application.CQRS;
using Common.Domain.Errors;
using Common.Domain.Results;
using FluentValidation;
using System.Application.Interfaces;
using System.Domain.Models;

namespace System.Application.Features.Tenant;

public class UpdateTenantCommandHandler(IRepository<TenantM> Repository)
    : ICommandHandler<UpdateTenantCommand, Result>
{
    public async Task<Result> Handle(
        UpdateTenantCommand command,
        CancellationToken cancellation = default)
    {
        var obj = await Repository.FindOneAsync(_ => _.TenantId == command.TenantId, cancellation);

        if (obj == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Tenant not found"));
        }

        obj.TenantName = command.TenantName;

        Repository.Update(obj);
        await Repository.SaveChangesAsync(cancellation);

        return Result.Success(obj);
    }
}

public sealed record UpdateTenantCommand(
    int TenantId,
    string TenantName);

public class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidator()
    {
        RuleFor(_ => _.TenantId).NotEmpty();
        RuleFor(_ => _.TenantName).NotEmpty();
    }
}