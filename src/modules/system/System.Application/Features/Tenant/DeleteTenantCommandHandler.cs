using System.Domain.Entities.Tenants;

namespace System.Application.Features.Tenant;

public class DeleteTenantCommandHandler(IGenericRepository<TenantM> Repository)
    : ICommandHandler<DeleteTenantCommand>
{
    public async Task<Result> Handle(
        DeleteTenantCommand command,
        CancellationToken cancellationToken)
    {
        var obj = await Repository.FindOneAsync(_ => _.TenantId == command.TenantId, cancellationToken);

        if (obj is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Tenant not found."));
        }

        Repository.Delete(obj);
        await Repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record DeleteTenantCommand(int TenantId) : ICommand;