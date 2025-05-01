using Common.Application.CQRS;
using Common.Domain.Errors;
using Common.Domain.Results;
using FluentValidation;
using System.Application.Interfaces;
using System.Domain.Models;

namespace System.Application.Features.Roles;

public class GetRoleByIdQueryHandler(IRepository<RoleM> repository)
    : IQueryHandler<GetRoleByIdQuery, Result<GetRoleByIdQueryResult>>
{
    public async Task<Result<GetRoleByIdQueryResult>> Handle(
        GetRoleByIdQuery query,
        CancellationToken cancellation = default)
    {
        var Obj = await repository.FindOneAsync(_ => _.RoleId == query.RoleId, cancellation);

        return Obj is not null
            ? Result.Success(new GetRoleByIdQueryResult(Obj))
            : Result.Failure<GetRoleByIdQueryResult>(CustomError.NotFound("Not Found", "Role not found."));
    }
}

public sealed record GetRoleByIdQuery(int RoleId);

public sealed record GetRoleByIdQueryResult(RoleM Role);

public class GetRoleByIdValidator : AbstractValidator<GetRoleByIdQuery>
{
    public GetRoleByIdValidator()
    {
        RuleFor(_ => _.RoleId).NotNull().NotEmpty();
    }
}
