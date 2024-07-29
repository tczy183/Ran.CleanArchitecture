using Serilog;

using Web;

try
{
    var builder = WebApplication.CreateBuilder(args);
    Log.Information("Starting Host");
    Log.Information("Current Host Environment-【{EnvironmentEnvironmentName}】", builder.Environment.EnvironmentName);
    Log.Information("Current Host SelfUrl-【{SelfUrl}】", builder.Configuration["App:SelfUrl"]);
    builder.Host.UseAutofac()
        .UseSerilog(configureLogger: (hostingContext, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext();
        });
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