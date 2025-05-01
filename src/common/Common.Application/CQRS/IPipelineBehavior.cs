namespace Common.Application.CQRS;

public interface IPipelineBehavior<TRequest, TResponse>
{
    Task<TResponse> Handle(
        TRequest request,
        Func<Task<TResponse>> nextHandler,
        CancellationToken cancellationToken = default);
}