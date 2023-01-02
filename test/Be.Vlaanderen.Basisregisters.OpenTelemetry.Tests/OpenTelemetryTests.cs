using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Xunit;

namespace Be.Vlaanderen.Basisregisters.OpenTelemetry.Tests
{
    public class OpenTelemetryTests
    {
        [Fact]
        public void AddOpenTelemetryTracing()
        {
            var services = new ServiceCollection();
            services.AddOpenTelemetryTracing(nameof(OpenTelemetryTests), true);

            Assert.Contains(services, x => x.ServiceType == typeof(TracerProvider));
        }

        [Fact]
        public void AddOpenTelemetryMetrics()
        {
            var services = new ServiceCollection();
            services.AddOpenTelemetryMetrics(true);

            Assert.Contains(services, x => x.ServiceType == typeof(MeterProvider));
        }
    }
}
