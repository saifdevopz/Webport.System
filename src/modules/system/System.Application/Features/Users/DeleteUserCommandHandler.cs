using Common.Application.CQRS;
using Common.Application.Interfaces;
using Common.Domain.Errors;
using Common.Domain.Results;
using System.Domain.Models;

namespace System.Application.Features.Users;

public class DeleteUserCommandHandler(IGenericRepository<UserM> Repository)
    : ICommandHandler<DeleteUserCommand, Result>
{
    public async Task<Result> Handle(
        DeleteUserCommand command,
        CancellationToken cancellation = default)
    {
        var obj = await Repository.FindOneAsync(_ => _.UserId == command.UserId, cancellation);

        if (obj is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "User not found."));
        }

        Repository.Delete(obj);
        await Repository.SaveChangesAsync(cancellation);

        return Result.Success();
    }
}

public sealed record DeleteUserCommand(int UserId);