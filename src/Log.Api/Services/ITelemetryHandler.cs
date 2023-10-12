namespace Log.Api.Services;

public interface ITelemetryHandler
{
    Task TrackEventAsync<T>(string eventName, T model, CancellationToken cancellationToken);
    Task TrackMetricAsync(string metricName, double metricValue, CancellationToken cancellationToken);
}