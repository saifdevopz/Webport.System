using FluentValidation;
using Parent.Domain.Inventory.Category;

namespace Parent.Application.Features.Category;

public sealed class CreateCategoryCommandHandler(IGenericRepository<CategoryM> Repository)
    : ICommandHandler<CreateCategoryCommand>
{
    public async Task<Result> Handle(
        CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var obj = CategoryM.Create(command.CategoryCode, command.CategoryDesc);

        await Repository.AddAsync(obj);
        await Repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record CreateCategoryCommand(string CategoryCode, string CategoryDesc) : ICommand;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(_ => _.CategoryCode).NotEmpty();
        RuleFor(_ => _.CategoryDesc).NotEmpty();
    }
}