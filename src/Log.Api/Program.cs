using Log.Api.Data;
using Log.Api.Persistence;
using Log.Api.Services;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging.Console;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.ApplicationInsights;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Adding Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Adding Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader();
    });
});

//Takes connectionstring from appsettings - ApplicationInsights section
builder.Services.AddApplicationInsightsTelemetry();

builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration)
    .WriteTo.ApplicationInsights(new TelemetryConfiguration
    {
        ConnectionString = hostingContext.Configuration.GetValue<string>("ApplicationInsights:ConnectionString")
    },TelemetryConverter.Traces)
);

// Adding Logging Providers
builder.Services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

//builder.Logging.AddApplicationInsights(Configuration.GetValue<string>("ApplicationInsights:ConnectionString"));

// builder.Logging.AddSimpleConsole(options =>
// {
//     options.ColorBehavior = LoggerColorBehavior.Enabled;
//     options.IncludeScopes = true;
// });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
