namespace Blazor.Server.Common.Results;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
}

public static class Result
{
    public static Result<T> Success<T>(T data) => new() { IsSuccess = true, Data = data };
    public static Result<T> Fail<T>(string error) => new() { IsSuccess = false, Error = error };
}