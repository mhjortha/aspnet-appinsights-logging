using BlazorApplicationInsights;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Log.Client;
using Log.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

if (builder.HostEnvironment.IsDevelopment())
{
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5146") });
}
else
{
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://app-backend-test-logging.azurewebsites.net") });
}

// Add Logging Providers
builder.Services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

builder.Services.AddBlazorApplicationInsights(async applicationInsights =>
{
    var telemetryItem = new TelemetryItem()
    {
        Tags = new Dictionary<string, object>()
        {
            { "ai.cloud.role", "SPA" },
            { "ai.cloud.roleInstance", "Blazor Wasm" },
        }
    };
    await applicationInsights.AddTelemetryInitializer(telemetryItem);
});

await builder.Build().RunAsync();
