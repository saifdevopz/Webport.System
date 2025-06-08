using FluentValidation;
using System.Domain.Models;

namespace System.Application.Features.Tenant;

public class UpdateTenantCommandHandler(IGenericRepository<TenantM> Repository)
    : ICommandHandler<UpdateTenantCommand>
{
    public async Task<Result> Handle(
        UpdateTenantCommand command,
        CancellationToken cancellationToken)
    {
        var obj = await Repository.FindOneAsync(_ => _.TenantId == command.TenantId, cancellationToken);

        if (obj == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Tenant not found"));
        }

        obj.TenantName = command.TenantName;

        Repository.Update(obj);
        await Repository.SaveChangesAsync(cancellationToken);

        return Result.Success(obj);
    }
}

public sealed record UpdateTenantCommand(
    int TenantId,
    string TenantName) : ICommand;

public class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidator()
    {
        RuleFor(_ => _.TenantId).NotEmpty();
        RuleFor(_ => _.TenantName).NotEmpty();
    }
}