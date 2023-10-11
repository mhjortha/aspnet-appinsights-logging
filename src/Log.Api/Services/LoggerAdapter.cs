namespace Log.Api.Services;

public class LoggerAdapter<T> : ILoggerAdapter<T>
{
    private readonly Serilog.ILogger _logger;

    public LoggerAdapter(Serilog.ILogger logger)
    {
        _logger = logger;
    }
    
    public void LogDebug(string? message, params object?[] args)
    {
        _logger.Debug(message, args);
    }
    
    public void LogCritical(string? message, params object?[] args)
    {
        _logger.Fatal(message, args);
    }
    
    public void LogTrace(string? message, params object?[] args)
    {
        _logger.Verbose(message, args);
    }

    public void LogInformation(string? message, params object?[] args)
    {
        _logger.Information(message, args);
    }

    public void LogError(string? message, params object?[] args)
    {
        _logger.Error(message, args);
    }

    public void LogWarning(string? message, params object?[] args)
    {
        _logger.Warning(message, args);
    }
}