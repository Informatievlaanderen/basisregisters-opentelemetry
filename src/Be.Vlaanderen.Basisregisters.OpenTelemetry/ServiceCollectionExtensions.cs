using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Be.Vlaanderen.Basisregisters.OpenTelemetry
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenTelemetryTracing(this IServiceCollection services, string? serviceName, bool isDevelopment = false)
        {
            services.AddOpenTelemetryTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .ConfigureResource(resourceBuilder => resourceBuilder.BuildOpenTelemetryResource(serviceName))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter();

                if (isDevelopment)
                {
                    tracerProviderBuilder.AddConsoleExporter();
                }
            });

            return services;
        }

        public static IServiceCollection AddOpenTelemetryMetrics(this IServiceCollection services, bool addPrometheusEndpoint = false)
        {
            services.AddOpenTelemetryMetrics(meterProviderBuilder =>
            {
                meterProviderBuilder
                    .AddRuntimeInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter();

                if (addPrometheusEndpoint)
                {
                    meterProviderBuilder.AddPrometheusExporter();
                }
            });

            return services;
        }

        internal static IServiceCollection AddOpenTelemetryLogging(this IServiceCollection services)
        {
            services.Configure<OpenTelemetryLoggerOptions>(opt =>
            {
                opt.IncludeScopes = true;
                opt.ParseStateValues = true;
                opt.IncludeFormattedMessage = true;
            });
            
            return services;
        }
    }
}
