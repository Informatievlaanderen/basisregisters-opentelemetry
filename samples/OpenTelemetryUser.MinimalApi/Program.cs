using System;
using Be.Vlaanderen.Basisregisters.OpenTelemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OpenTelemetryUser.MinimalApi
{
    public class Program
    {
        protected Program()
        { }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{Environment.MachineName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            builder
                .AddOpenTelemetryTracing(typeof(Program).Namespace, displayName => displayName.StartsWith("/health") || displayName.StartsWith("/metrics"), isDevelopment:builder.Environment.IsDevelopment())
                .AddOpenTelemetryMetrics(true)
                .AddOpenTelemetryLogging()
                .Services.AddHealthChecks();

            var app = builder.Build();

            app.MapGet("/", () => "Hello");
            app.MapHealthChecks("/health");

            app.UseOpenTelemetryPrometheusScrapingEndpoint();

            app.Run();
        }
    }
}
