using Microsoft.ApplicationInsights;

namespace Log.Api.Services;

public class TelemetryHandler : ITelemetryHandler
{
    private readonly TelemetryClient _telemetryClient;
    public TelemetryHandler(TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient;
    }
    public async Task TrackMetricAsync(string metricName, double metricValue, CancellationToken cancellationToken)
    {
        _telemetryClient.TrackMetric(metricName, metricValue);
        await _telemetryClient.FlushAsync(cancellationToken);
    }
    public async Task TrackEventAsync<T>(string eventName, T model, CancellationToken cancellationToken)
    {
        var dict = ConvertToJsonDictionary(model);
        
        _telemetryClient.TrackEvent(eventName, dict);
        await _telemetryClient.FlushAsync(cancellationToken);
    }
    private Dictionary<string, string> ConvertToJsonDictionary<T>(T model)
    {
        var result = new Dictionary<string, string> {{model.GetType().Name, System.Text.Json.JsonSerializer.Serialize(model)}};

        return result;
    }
}
