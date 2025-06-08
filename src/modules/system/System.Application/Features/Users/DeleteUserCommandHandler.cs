using System.Domain.Models;

namespace System.Application.Features.Users;

public class DeleteUserCommandHandler(IGenericRepository<UserM> Repository)
    : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> Handle(
        DeleteUserCommand command,
        CancellationToken cancellationToken)
    {
        var obj = await Repository.FindOneAsync(_ => _.UserId == command.UserId, cancellationToken);

        if (obj is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "User not found."));
        }

        Repository.Delete(obj);
        await Repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record DeleteUserCommand(int UserId) : ICommand;