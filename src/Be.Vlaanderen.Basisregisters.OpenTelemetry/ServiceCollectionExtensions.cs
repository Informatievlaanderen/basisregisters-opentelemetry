﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Be.Vlaanderen.Basisregisters.OpenTelemetry
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenTelemetryTracing(this IServiceCollection services, string? serviceName, Predicate<string>? noTracePredicate = null,
            IEnumerable<string>? additionalSources = null, bool isDevelopment = false)
        {
            services.AddOpenTelemetryTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .ConfigureResource(resourceBuilder => resourceBuilder.BuildOpenTelemetryResource(serviceName))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter();

                if (noTracePredicate is not null)
                {
                    tracerProviderBuilder.AddProcessor(new NoTraceProcessor(noTracePredicate));
                }

                if (additionalSources is not null)
                {
                    foreach (var additionalSource in additionalSources)
                    {
                        tracerProviderBuilder.AddSource(additionalSource);
                    }
                }

                if (isDevelopment)
                {
                    tracerProviderBuilder.AddConsoleExporter();
                }
            });

            return services;
        }

        public static IServiceCollection AddOpenTelemetryMetrics(this IServiceCollection services, bool addPrometheusEndpoint = false, IEnumerable<string>? additionalMeters = null,
            IEnumerable<(string Name, ExplicitBucketHistogramConfiguration Configuration)>? additionalViews = null)
        {
            services.AddOpenTelemetryMetrics(meterProviderBuilder =>
            {
                meterProviderBuilder
                    .AddRuntimeInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter();

                if (additionalMeters is not null)
                {
                    foreach (var additionalMeter in additionalMeters)
                    {
                        meterProviderBuilder.AddMeter(additionalMeter);
                    }
                }

                if (additionalViews is not null)
                {
                    foreach (var additionalView in additionalViews)
                    {
                        meterProviderBuilder.AddView(additionalView.Name, additionalView.Configuration);
                    }
                }

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
