namespace Log.Api.Services;

public interface ILoggerAdapter<T>
{
    public void LogDebug(string? message, params object?[] args);
    public void LogInformation(string? message, params object?[] args);
    public void LogError(string? message, params object?[] args);
    public void LogWarning(string? message, params object?[] args);
    public void LogCritical(string? message, params object?[] args);
    public void LogTrace(string? message, params object?[] args);
}