using Application.WeatherForecasts.Queries.GetWeatherForecasts;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
    : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger = logger;

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        return await mediator.Send(new GetWeatherForecastsQuery());
    }
}