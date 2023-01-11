using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using System;
using System.Collections.Generic;
using OpenTelemetry.Metrics;

namespace Be.Vlaanderen.Basisregisters.OpenTelemetry
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddOpenTelemetryTracing(this WebApplicationBuilder builder, string? serviceName, Predicate<string>? noTracePredicate = null,
            IEnumerable<string>? additionalSources = null, bool isDevelopment = false)
        {
            builder.Services.AddOpenTelemetryTracing(serviceName, noTracePredicate, additionalSources, isDevelopment);

            return builder;
        }

        public static WebApplicationBuilder AddOpenTelemetryMetrics(this WebApplicationBuilder builder, bool addPrometheusEndpoint = false, IEnumerable<string>? additionalMeters = null,
        IEnumerable<(string Name, ExplicitBucketHistogramConfiguration Configuration)>? additionalViews = null)
        {
            builder.Services.AddOpenTelemetryMetrics(addPrometheusEndpoint, additionalMeters, additionalViews);

            return builder;
        }

        //public static WebApplicationBuilder AddOpenTelemetryLogging(this WebApplicationBuilder builder, string? serviceName, bool clearLoggingProviders = true)
        //{
        //    if (clearLoggingProviders)
        //    {
        //        builder.Logging.ClearProviders();
        //    }

        //    builder.Logging.AddOpenTelemetry(options =>
        //    {
        //        options
        //            .ConfigureResource(resourceBuilder => resourceBuilder.BuildOpenTelemetryResource(serviceName))
        //            .AddConsoleExporter();
        //    });

        //    builder.Services.AddOpenTelemetryLogging();

        //    return builder;
        //}
    }
}
