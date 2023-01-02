using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;

namespace Be.Vlaanderen.Basisregisters.OpenTelemetry
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddOpenTelemetryTracing(this WebApplicationBuilder builder, string? serviceName, bool isDevelopment = false)
        {
            builder.Services.AddOpenTelemetryTracing(serviceName, isDevelopment);

            return builder;
        }

        public static WebApplicationBuilder AddOpenTelemetryMetrics(this WebApplicationBuilder builder, bool addPrometheusEndpoint = false)
        {
            builder.Services.AddOpenTelemetryMetrics(addPrometheusEndpoint);

            return builder;
        }

        public static WebApplicationBuilder AddOpenTelemetryLogging(this WebApplicationBuilder builder, string? serviceName, bool clearLoggingProviders = true)
        {
            if (clearLoggingProviders)
            {
                builder.Logging.ClearProviders();
            }

            builder.Logging.AddOpenTelemetry(options =>
            {
                options
                    .ConfigureResource(resourceBuilder => resourceBuilder.BuildOpenTelemetryResource(serviceName))
                    .AddConsoleExporter();
            });

            builder.Services.AddOpenTelemetryLogging();

            return builder;
        }
    }
}
