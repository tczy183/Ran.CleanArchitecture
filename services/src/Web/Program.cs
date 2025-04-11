using Ran.Core.AspNetCore.Extensions.Builder;
using Ran.Core.Extensions.DependencyInjection;
using Serilog;
using Web;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("serilog.json", optional: true, reloadOnChange: true)
    .Build();

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

try
{
    Log.ForContext<Program>().Information("Starting Web");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    await builder.Services.AddApplicationAsync<WebModule>();
    var app = builder.Build();
    await app.InitializeApplicationAsync();
    await app.RunAsync();
}
catch (Exception e)
{
    Log.ForContext<Program>().Fatal(e, "An error occurred while starting the Web");
}
finally
{
    await Log.CloseAndFlushAsync();
}
