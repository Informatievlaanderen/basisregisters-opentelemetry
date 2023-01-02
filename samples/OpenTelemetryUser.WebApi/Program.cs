using Be.Vlaanderen.Basisregisters.Api;
using Be.Vlaanderen.Basisregisters.OpenTelemetry;
using Microsoft.AspNetCore.Hosting;

namespace OpenTelemetryUser.WebApi
{
    public static class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => new WebHostBuilder()
                .AddOpenTelemetryLogging(typeof(Startup).Namespace)
                .UseDefaultForApi<Startup>(
                    new ProgramOptions
                    {
                        Logging =
                        {
                            WriteTextToConsole = false,
                            WriteJsonToConsole = false
                        },
                        Runtime =
                        {
                            CommandLineArgs = args
                        }
                    });
    }
}