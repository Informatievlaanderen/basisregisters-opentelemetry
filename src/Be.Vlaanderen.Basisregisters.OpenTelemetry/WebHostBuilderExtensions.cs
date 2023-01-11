using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;

namespace Be.Vlaanderen.Basisregisters.OpenTelemetry
{
    public static class WebHostBuilderExtensions
    {
        //public static WebHostBuilder AddOpenTelemetryLogging(this WebHostBuilder builder, string? serviceName, bool clearLoggingProviders = true)
        //{
        //    if (clearLoggingProviders)
        //    {
        //        builder.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders());
        //    }

        //    builder.ConfigureLogging(loggingBuilder =>
        //    {
        //        loggingBuilder.AddOpenTelemetry(options =>
        //        {
        //            options
        //                .ConfigureResource(resourceBuilder => resourceBuilder.BuildOpenTelemetryResource(serviceName))
        //                .AddConsoleExporter();
        //        });
        //    });

        //    builder.ConfigureServices((_, services) => services.AddOpenTelemetryLogging());

        //    return builder;
        //}
    }
}
