namespace Common.Infrastructure.Authentication;

#pragma warning disable CA1032 // Implement standard exception constructors
public sealed class CustomException(string requestName, string? error = default, Exception? innerException = default)
#pragma warning restore CA1032 // Implement standard exception constructors
    : Exception("Application exception", innerException)
{

    public string RequestName { get; } = requestName;

    public string? Error { get; } = error;

}