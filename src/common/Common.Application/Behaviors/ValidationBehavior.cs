using Common.Application.CQRS;
using Common.Domain.Errors;
using Common.Domain.Results;
using FluentValidation;
using FluentValidation.Results;
using System.Reflection;

namespace Common.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    // Main pipeline method that handles validation logic
    public async Task<TResponse> Handle(
        TRequest request,
        Func<Task<TResponse>> nextHandler,  // The next delegate in the pipeline (could be another behavior or the final handler)
        CancellationToken cancellationToken = default)  // Cancellation support
    {
        // Validate the request and get any validation errors
        ValidationFailure[] validationFailures = await ValidateAsync(request, cancellationToken);

        // If there are no validation errors, continue to the next handler immediately
        if (validationFailures.Length == 0)
        {
            return await nextHandler();
        }

        // If the response is a generic Result<T>, we want to build a ValidationFailure Result
        if (typeof(TResponse).IsGenericType &&
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            // Get the generic type inside Result<T>
            Type resultType = typeof(TResponse).GetGenericArguments()[0];

            // Find the static ValidationFailure method from Result<T>
            MethodInfo? failureMethod = typeof(Result<>)
                .MakeGenericType(resultType)
                .GetMethod(nameof(Result<object>.ValidationFailure));

            if (failureMethod is not null)
            {
                // Dynamically invoke the ValidationFailure method and return it
                return (TResponse)failureMethod.Invoke(null, [CreateValidationError(validationFailures)])!;
            }
        }
        // If the response is a non-generic Result (no <T>), build a simple failure Result
        else if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(CreateValidationError(validationFailures));
        }

        // If neither Result nor Result<T>, just throw a FluentValidation.ValidationException
        throw new ValidationException(validationFailures);
    }

    // Helper method to perform the actual validation
    private async Task<ValidationFailure[]> ValidateAsync(TRequest request, CancellationToken cancellationToken)
    {
        // If there are no validators, skip validation
        if (!_validators.Any())
        {
            return [];
        }

        // Create a validation context using the incoming request
        ValidationContext<TRequest> context = new(request);

        // Validate the request against all validators asynchronously
        ValidationResult[] validationResults = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        // Collect all validation errors from the results
        var failures = validationResults
            .Where(r => !r.IsValid)  // Only take invalid results
            .SelectMany(r => r.Errors)  // Flatten all errors into one list
            .ToArray();

        return failures;
    }

    // Helper method to create a ValidationError object from a list of FluentValidation failures
    private static ValidationError CreateValidationError(ValidationFailure[] failures)
    {
        return new ValidationError(
            failures.Select(f => CustomError.Problem(f.ErrorCode, f.ErrorMessage)).ToArray()
        );
    }
}
