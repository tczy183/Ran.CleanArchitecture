using Application.Events;
using Application.WeatherForecasts.Queries.GetWeatherForecasts;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Volo.Abp.EventBus.Local;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(
    ILogger<WeatherForecastController> logger,
    IMediator mediator,
    ILocalEventBus localEventBus,
    IOptions<AbpLocalEventBusOptions> abpLocalEventBusOptions)
    : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger = logger;
    private readonly AbpLocalEventBusOptions _abpLocalEventBusOptions = abpLocalEventBusOptions.Value;

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        return await mediator.Send(new GetWeatherForecastsQuery());
    }

    [HttpPost]
    public async Task PostTest()
    {
        _logger.LogDebug("PostTest");
        _logger.LogInformation("PostTest");
        _logger.LogError("PostTest");
        await localEventBus.PublishAsync(new Test1Event { Name = "TestEvent" });
        await localEventBus.PublishAsync(new Test2Event { Name = "TestEvent" });
    }
}