using FluentValidation;
using Tenant.Domain.Entities.INCategory;

namespace Tenant.Application.Features.INCategory;

public class UpdateCategoryCommandHandler(IGenericRepository<INCategoryM> Repository)
    : ICommandHandler<UpdateCategoryCommand, UpdateCategoryResult>
{
    public async Task<Result<UpdateCategoryResult>> Handle(
        UpdateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var obj = await Repository.FindOneAsync(_ => _.CategoryId == command.CategoryId, cancellationToken);

        if (obj == null)
        {
            return Result.Failure<UpdateCategoryResult>(CustomError.NotFound("Not Found", "Record not found."));
        }

        obj.CategoryDesc = command.CategoryDesc;

        Repository.Update(obj);
        await Repository.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateCategoryResult(obj));
    }
}

public sealed record UpdateCategoryCommand(int CategoryId, string CategoryDesc) : ICommand<UpdateCategoryResult>;

public sealed record UpdateCategoryResult(INCategoryM Result);

public class UpdateRoleCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(_ => _.CategoryId).NotEmpty();
        RuleFor(_ => _.CategoryDesc).NotEmpty();
    }
}