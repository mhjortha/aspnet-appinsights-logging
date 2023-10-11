using Log.Api.Data;
using Log.Api.Persistence;
using Log.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.ApplicationInsights;

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
        policy.WithOrigins("http://localhost:5043", "https://localhost", "https://app-frontend-test-logging.azurewebsites.net")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Adding Logging Providers
builder.Services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

builder.Services.AddApplicationInsightsTelemetry();

builder.Logging.AddApplicationInsights(
    configureTelemetryConfiguration: (config) => 
        config.ConnectionString = builder.Configuration.GetValue<string>("ApplicationInsights:ConnectionString"),
    configureApplicationInsightsLoggerOptions: (options) =>
    {
        options.IncludeScopes = true;
        options.TrackExceptionsAsExceptionTelemetry = true;
    }
);

builder.Logging.AddSimpleConsole(options =>
{
    options.ColorBehavior = LoggerColorBehavior.Enabled;
    options.IncludeScopes = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
