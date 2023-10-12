namespace Log.Api.Options;

public sealed class ApplicationInsightsOptions
{
    public const string Position = "ApplicationInsights";
    public string ConnectionString { get; set; }
}