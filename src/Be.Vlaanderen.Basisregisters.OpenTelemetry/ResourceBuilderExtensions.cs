using System;
using System.Reflection;
using OpenTelemetry.Resources;

namespace Be.Vlaanderen.Basisregisters.OpenTelemetry
{
    internal static class ResourceBuilderExtensions
    {
        public static void BuildOpenTelemetryResource(this ResourceBuilder resourceBuilder, string? serviceName)
        {
            var assemblyVersion = Assembly.GetExecutingAssembly()
                .GetName().Version?.ToString() ?? "0.0.0";

            resourceBuilder
                .AddService(serviceName ?? "Service", serviceVersion: assemblyVersion, serviceInstanceId: Environment.MachineName)
                .Build();
        }
    }
}
