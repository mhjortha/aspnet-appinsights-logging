namespace Log.Api.Services;

public interface ITelemetryHandler
{
    Task TrackEvent<T>(string eventName, T model, CancellationToken cancellationToken);
}