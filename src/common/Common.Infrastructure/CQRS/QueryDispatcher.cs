using Common.Application.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.CQRS;

public class QueryDispatcher(IServiceScopeFactory serviceScopeFactory) : IQueryDispatcher
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    public async Task<TQueryResult> Dispatch<TQuery, TQueryResult>(TQuery query, CancellationToken cancellation = default)
    {
        // Create a new service scope to resolve dependencies.
        using var scope = _serviceScopeFactory.CreateAsyncScope();

        // Get the service provider from the scoped container.
        var provider = scope.ServiceProvider;

        // Resolve the command handler from DI.
        var handler = provider.GetRequiredService<IQueryHandler<TQuery, TQueryResult>>();

        // Resolve all pipeline behaviors from DI, reversing to maintain order.
        var behaviors = provider
                .GetServices<IPipelineBehavior<TQuery, TQueryResult>>()
                .Reverse()  // Reverse the order to ensure the first behavior is applied first.
                .ToList();

        // Define the next delegate, which is initially the handler execution.
        Func<Task<TQueryResult>> nextHandler = () => handler.Handle(query, cancellation);

        // Loop through each behavior and wrap it around the handler execution.
        foreach (var behavior in behaviors)
        {
            var next = nextHandler;  // Capture the current next handler in a variable.

            // Update the next handler to apply the current behavior.
            nextHandler = () => behavior.Handle(query, next, cancellation);
        }

        // Execute the entire pipeline (including behaviors) and return the result.
        return await nextHandler();
    }
}
