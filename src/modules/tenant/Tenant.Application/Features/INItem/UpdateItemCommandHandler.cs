using FluentValidation;
using Tenant.Domain.Entities.INItem;

namespace Tenant.Application.Features.INItem;

public class UpdateItemCommandHandler(IGenericRepository<INItemM> Repository)
    : ICommandHandler<UpdateItemCommand>
{
    public async Task<Result> Handle(
        UpdateItemCommand command,
        CancellationToken cancellationToken)
    {
        var obj = await Repository.FindOneAsync(_ => _.ItemId == command.ItemId, cancellationToken);

        if (obj == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Record not found."));
        }

        obj.ItemDesc = command.ItemDesc;

        Repository.Update(obj);
        await Repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record UpdateItemCommand(int ItemId, string ItemDesc) : ICommand;

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(_ => _.ItemId).NotEmpty();
        RuleFor(_ => _.ItemDesc).NotEmpty();
    }
}