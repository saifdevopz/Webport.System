using Common.Application.CQRS;
using Common.Domain.Errors;
using Common.Domain.Results;
using FluentValidation;
using System.Application.Interfaces;
using System.Domain.Models;

namespace System.Application.Features.Users;

public class UpdateUserCommandHandler(IRepository<UserM> Repository)
    : ICommandHandler<UpdateUserCommand, Result>
{
    public async Task<Result> Handle(
        UpdateUserCommand command,
        CancellationToken cancellation = default)
    {
        var Obj = await Repository.FindOneAsync(_ => _.UserId == command.UserId, cancellation);

        if (Obj == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "User not found"));
        }

        Obj.FullName = command.FullName;

        Repository.Update(Obj);
        await Repository.SaveChangesAsync(cancellation);

        return Result.Success(Obj);
    }
}

public sealed record UpdateUserCommand(
    int UserId,
    string FullName);

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(_ => _.UserId).NotNull().NotEmpty();
        RuleFor(_ => _.FullName).NotNull().NotEmpty();
    }
}
