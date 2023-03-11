// See https://aka.ms/new-console-template for more information
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Storage.Shared.Protocol;

namespace WebJobsSDKSample
{
    class Program
    {
        static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                       .MinimumLevel.Debug()
                       .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                       .Enrich.FromLogContext()
                       .WriteTo.Console()
                       .CreateBootstrapLogger();
            try
            {
                var builder = Host.CreateDefaultBuilder();
                builder.ConfigureWebJobs(b =>
                {
                    b.AddAzureStorageCoreServices();
                    b.AddTimers();
                });
                builder.ConfigureLogging((context, b) =>
                {
                    b.AddConsole();
                    // If the key exists in settings, use it to enable Application Insights.
                    string instrumentationKey = context.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]!;
                    if (!string.IsNullOrEmpty(instrumentationKey))
                    {
                        b.AddApplicationInsightsWebJobs(o => o.InstrumentationKey = instrumentationKey);
                    }
                });
                builder.ConfigureWebJobs(b =>
                {
                    b.AddAzureStorageCoreServices();
                });
                builder.UseSerilog((context, services, loggerConfiguration) =>
                {
                    loggerConfiguration.WriteTo.ApplicationInsights(services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces);
                    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
                    loggerConfiguration.Enrich.FromLogContext();
                    loggerConfiguration.WriteTo.Console();
                    });
                builder.UseEnvironment(Microsoft.Extensions.Hosting.Environments.Development);
                builder.ConfigureServices((context, service) =>
                {
                    service.AddTransient(typeof(WebJobTrigger));
                });
                Log.Information("Starting host");
                var host = builder.Build();
                using (host)
                {
                    await host.RunAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}