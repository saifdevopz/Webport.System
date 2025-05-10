using Common.Application.CQRS;
using Common.Application.Interfaces;
using Common.Domain.Results;
using FluentValidation;
using Parent.Domain.Inventory.Category;

namespace Parent.Application.Features.Category;

public sealed class CreateCategoryCommandHandler(IGenericRepository<CategoryM> Repository)
    : ICommandHandler<CreateCategoryCommand, Result>
{
    public async Task<Result> Handle(
        CreateCategoryCommand command,
        CancellationToken cancellation = default)
    {
        var obj = CategoryM.Create(command.CategoryCode, command.CategoryDesc);

        await Repository.AddAsync(obj);
        await Repository.SaveChangesAsync(cancellation);

        return Result.Success();
    }
}

public sealed record CreateCategoryCommand(string CategoryCode, string CategoryDesc);

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(_ => _.CategoryCode).NotEmpty();
        RuleFor(_ => _.CategoryDesc).NotEmpty();
    }
}