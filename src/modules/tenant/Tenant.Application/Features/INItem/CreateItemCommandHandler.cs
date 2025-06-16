using FluentValidation;
using Tenant.Domain.Entities.INItem;

namespace Tenant.Application.Features.INItem;

public class CreateItemCommandHandler(IGenericRepository<INItemM> Repository)
    : ICommandHandler<CreateItemCommand>
{
    public async Task<Result> Handle(
        CreateItemCommand command,
        CancellationToken cancellationToken)
    {
        var obj = INItemM.Create(command.ItemCode, command.ItemDesc);

        await Repository.AddAsync(obj);
        await Repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record CreateItemCommand(string ItemCode, string ItemDesc) : ICommand;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(_ => _.ItemCode).NotEmpty();
        RuleFor(_ => _.ItemDesc).NotEmpty();
    }
}