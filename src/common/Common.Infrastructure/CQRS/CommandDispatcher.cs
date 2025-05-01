using Common.Application.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.CQRS;

public class CommandDispatcher(IServiceScopeFactory serviceScopeFactory) : ICommandDispatcher
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    public async Task<TCommandResult> Dispatch<TCommand, TCommandResult>
        (TCommand command,
        CancellationToken cancellation = default)
    {
        // Create a new service scope to resolve dependencies.
        using var scope = _serviceScopeFactory.CreateAsyncScope();

        // Get the service provider from the scoped container.
        var provider = scope.ServiceProvider;

        // Resolve the command handler from DI.
        var handler = provider.GetRequiredService<ICommandHandler<TCommand, TCommandResult>>();

        // Resolve all pipeline behaviors from DI, reversing to maintain order.
        var behaviors = provider
                .GetServices<IPipelineBehavior<TCommand, TCommandResult>>()
                .Reverse()  // Reverse the order to ensure the first behavior is applied first.
                .ToList();

        // Define the next delegate, which is initially the handler execution.
        Func<Task<TCommandResult>> nextHandler = () => handler.Handle(command, cancellation);

        // Loop through each behavior and wrap it around the handler execution.
        foreach (var behavior in behaviors)
        {
            var next = nextHandler;  // Capture the current next handler in a variable.

            // Update the next handler to apply the current behavior.
            nextHandler = () => behavior.Handle(command, next, cancellation);
        }

        // Execute the entire pipeline (including behaviors) and return the result.
        return await nextHandler();
    }
}
