using Log.Api.Data;
using Log.Api.Persistence;
using Log.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Log.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services, 
        IConfiguration configuration
        )
    {
        // Add services to the container.
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Adding DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        });

        // Adding Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // Adding Cors
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // Takes connectionstring from appsettings - ApplicationInsights section as default
        services.AddApplicationInsightsTelemetry(options =>
        {
            options.ConnectionString = configuration.GetValue<string>("ApplicationInsights:ConnectionString");
        });

        // Adding Logging Providers
        services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
        services.AddSingleton<ITelemetryHandler, TelemetryHandler>();

        return services;
    }
}