namespace Blazor.Common.Results;

public class ApiResult<T>
{
    public required bool IsSuccess { get; set; }
    public required bool IsFailure { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
}