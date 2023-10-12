using Log.Api.Options;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;

namespace Log.Api.Services;

public class TelemetryHandler : ITelemetryHandler
{

    private readonly TelemetryClient _telemetryClient;
    
    public TelemetryHandler(IOptions<ApplicationInsightsOptions> options)
    {
        _telemetryClient = new TelemetryClient(new TelemetryConfiguration()
        {
            ConnectionString = options.Value.ConnectionString
        });
    }
    
    public async Task TrackEvent<T>(string eventName, T model, CancellationToken cancellationToken)
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
