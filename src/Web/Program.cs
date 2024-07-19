using Infrastructure;

using Serilog;
using Serilog.Events;

using Web;

Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
     .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File("Logs/logs.txt"))
    .WriteTo.Async(c => c.Console())
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    Log.Information("Starting Host");
    Log.Information("Current Host Environment-【{EnvironmentEnvironmentName}】", builder.Environment.EnvironmentName);
    Log.Information("Current Host SelfUrl-【{SelfUrl}】", builder.Configuration["App:SelfUrl"]);

    builder.Host.UseAutofac()
        .UseSerilog();
    await builder.Services.AddApplicationAsync<WebModule>();
    var app = builder.Build();
    await app.InitializeApplicationAsync();
    await app.RunAsync();
}
catch (Exception ex)
{
    if (ex is HostAbortedException)
    {
        throw;
    }

    Log.Fatal(ex, "Host terminated unexpectedly!");
}
finally
{
    Log.CloseAndFlush();
}