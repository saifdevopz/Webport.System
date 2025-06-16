
using FluentValidation;
using Tenant.Domain.Entities.INCategory;

namespace Tenant.Application.Features.INCategory;

public class DeleteCategoryCommandHandler(IGenericRepository<INCategoryM> Repository)
    : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(
        DeleteCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var obj = await Repository.FindOneAsync(_ => _.CategoryId == command.CategoryId, cancellationToken);

        if (obj is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Record not found."));
        }

        Repository.Delete(obj);
        await Repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record DeleteCategoryCommand(int CategoryId) : ICommand;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(_ => _.CategoryId).NotEmpty();
    }
}