using Common.Domain.Errors;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Common.Domain.Results;

public class Result
{
    public Result(bool isSuccess, CustomError error)
    {
        if ((isSuccess && error != CustomError.None) || (!isSuccess && error == CustomError.None))
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = isSuccess ? null : error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public CustomError? Error { get; }

    public static Result<T> Success<T>(T data) => data is not null
        ? new Result<T>(data, true, CustomError.None)
        : throw new ArgumentNullException(nameof(data), "Value cannot be null for success");

    public static Result Success() => new(true, CustomError.None);
    public static Result Failure(CustomError error) => new(false, error);
    public static Result<T> Failure<T>(CustomError error) => new(default, false, error);

    public static Result<T> ToResult<T>(T? data)
    {
        return data is not null
            ? Success(data)
            : Failure<T>(CustomError.NullValue);
    }

}

public class Result<T>(T? data, bool isSuccess, CustomError error) : Result(isSuccess, error)
{
    private readonly T? _data = data;

    [JsonPropertyOrder(99)]
    [NotNull]
    public T data => IsSuccess
        ? _data!
        : throw new InvalidOperationException("Cannot access the value of a failed result.");

    [SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
    public static Result<T> ValidationFailure(CustomError error)
    {
        return new(default, false, error);
    }
}

