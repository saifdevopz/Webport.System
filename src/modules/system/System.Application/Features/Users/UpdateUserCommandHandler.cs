using Common.Application.CQRS;
using Common.Application.Interfaces;
using Common.Domain.Errors;
using Common.Domain.Results;
using FluentValidation;
using System.Domain.Models;

namespace System.Application.Features.Users;

public class UpdateUserCommandHandler(IGenericRepository<UserM> Repository)
    : ICommandHandler<UpdateUserCommand, Result>
{
    public async Task<Result> Handle(
        UpdateUserCommand command,
        CancellationToken cancellation = default)
    {
        var obj = await Repository.FindOneAsync(_ => _.UserId == command.UserId, cancellation);

        if (obj == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "User not found"));
        }

        obj.FullName = command.FullName;

        Repository.Update(obj);
        await Repository.SaveChangesAsync(cancellation);

        return Result.Success();
    }
}

public sealed record UpdateUserCommand(
    int UserId,
    string FullName);

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(_ => _.UserId).NotEmpty();
        RuleFor(_ => _.FullName).NotEmpty();
    }
}
