using Ran.Core.Ran.Modularity;
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

    builder.Services.ConfigureServiceCollection<WebModule>();
    var app = builder.Build();
    app.BuildApplicationBuilder(app);
    app.Run();
}
catch (Exception e)
{
    Log.ForContext<Program>().Fatal(e, "An error occurred while starting the Web");
}
finally
{
    Log.CloseAndFlush();
}
