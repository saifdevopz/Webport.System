using FluentValidation;
using Tenant.Domain.Entities.INItem;

namespace Tenant.Application.Features.INItem;

public class DeleteItemCommandHandler(IGenericRepository<INItemM> Repository)
    : ICommandHandler<DeleteItemCommand>
{
    public async Task<Result> Handle(
        DeleteItemCommand command,
        CancellationToken cancellationToken)
    {
        var obj = await Repository.FindOneAsync(_ => _.ItemId == command.ItemId, cancellationToken);

        if (obj is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Record not found."));
        }

        Repository.Delete(obj);
        await Repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record DeleteItemCommand(int ItemId) : ICommand;

public class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
{
    public DeleteItemCommandValidator()
    {
        RuleFor(_ => _.ItemId).NotEmpty();
    }
}