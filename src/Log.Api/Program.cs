using Log.Api;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation(builder.Configuration);

//Add serilog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration)
    .WriteTo.ApplicationInsights(new TelemetryConfiguration
    {
        ConnectionString = hostingContext.Configuration.GetValue<string>("ApplicationInsights:ConnectionString")
    },TelemetryConverter.Traces)
);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();
app.Run();
