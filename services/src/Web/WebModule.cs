using Application;
using Infrastructure;
using Ran.Core.Ran.Modularity;
using Ran.Core.Ran.Modularity.Abstractions;
using Ran.Core.Ran.Modularity.Attributes;
using Ran.EventBus;

namespace Web;

[DependsOn(typeof(ApplicationModule), typeof(InfrastructureModule), typeof(EventBusModule))]
public class WebModule : BaseModule
{
    public override void ConfigureServices(IApplicationConfigureServiceContext context)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        context.Services.AddEndpointsApiExplorer();
        context.Services.AddSwaggerGen();
        context.Services.AutoRegisterFromRanCore();
    }

    public override void OnApplicationInitialization(IApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        var endpoints = context.GetEndpointRouteBuilder();

        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        var summaries = new[]
        {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching",
        };

        endpoints
            .MapGet(
                "/weatherforecast",
                () =>
                {
                    var forecast = Enumerable
                        .Range(1, 5)
                        .Select(index => new WeatherForecast(
                            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            Random.Shared.Next(-20, 55),
                            summaries[Random.Shared.Next(summaries.Length)]
                        ))
                        .ToArray();
                    return forecast;
                }
                
            )
            .WithName("GetWeatherForecast")
            .WithOpenApi();
    }
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

