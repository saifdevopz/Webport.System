using FluentValidation;
using Tenant.Domain.Entities.INCategory;

namespace Tenant.Application.Features.INCategory;

public sealed class CreateCategoryCommandHandler(IGenericRepository<INCategoryM> Repository)
    : ICommandHandler<CreateCategoryCommand>
{
    public async Task<Result> Handle(
        CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var obj = INCategoryM.Create(command.CategoryCode, command.CategoryDesc);

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

