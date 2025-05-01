using Common.Application.CQRS;
using Common.Domain.Errors;
using Common.Domain.Results;
using FluentValidation;
using System.Application.Interfaces;
using System.Domain.Models;

namespace System.Application.Features.Users;

public class DeleteUserCommandHandler(IRepository<UserM> Repository)
    : ICommandHandler<DeleteUserCommand, Result>
{
    public async Task<Result> Handle(
        DeleteUserCommand command,
        CancellationToken cancellation = default)
    {
        var Obj = await Repository.FindOneAsync(_ => _.UserId == command.UserId, cancellation);

        if (Obj is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "User not found."));
        }

        Repository.Delete(Obj);
        await Repository.SaveChangesAsync(cancellation);

        return Result.Success();
    }
}

public sealed record DeleteUserCommand(int UserId);

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(_ => _.UserId).NotNull().NotEmpty();
    }
}