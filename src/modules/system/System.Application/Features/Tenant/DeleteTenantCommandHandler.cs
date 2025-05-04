using Common.Application.CQRS;
using Common.Domain.Errors;
using Common.Domain.Results;
using System.Application.Interfaces;
using System.Domain.Models;

namespace System.Application.Features.Tenant;

public class DeleteTenantCommandHandler(IRepository<TenantM> Repository)
    : ICommandHandler<DeleteTenantCommand, Result>
{
    public async Task<Result> Handle(
        DeleteTenantCommand command,
        CancellationToken cancellation = default)
    {
        var obj = await Repository.FindOneAsync(_ => _.TenantId == command.TenantId, cancellation);

        if (obj is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Tenant not found."));
        }

        Repository.Delete(obj);
        await Repository.SaveChangesAsync(cancellation);

        return Result.Success();
    }
}

public sealed record DeleteTenantCommand(int TenantId);