using FluentValidation;
using System.Domain.Models;

namespace System.Application.Features.Users;

public class UpdateUserCommandHandler(IGenericRepository<UserM> Repository)
    : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(
        UpdateUserCommand command,
        CancellationToken cancellationToken)
    {
        var obj = await Repository.FindOneAsync(_ => _.UserId == command.UserId, cancellationToken);

        if (obj == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "User not found"));
        }

        obj.FullName = command.FullName;

        Repository.Update(obj);
        await Repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record UpdateUserCommand(
    int UserId,
    string FullName) : ICommand;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(_ => _.UserId).NotEmpty();
        RuleFor(_ => _.FullName).NotEmpty();
    }
}
