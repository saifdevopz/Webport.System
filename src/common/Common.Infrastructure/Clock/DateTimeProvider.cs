namespace Common.Infrastructure.Clock;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}