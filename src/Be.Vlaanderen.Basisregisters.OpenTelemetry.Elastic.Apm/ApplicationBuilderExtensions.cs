using Elastic.Apm.AspNetCore;
using Elastic.Apm.AspNetCore.DiagnosticListener;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.EntityFrameworkCore;
using Elastic.Apm.SqlClient;
using ElasticApm.MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Be.Vlaanderen.Basisregisters.OpenTelemetry.ElasticApm;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseElasticApm(this IApplicationBuilder applicationBuilder, IConfiguration configuration) => applicationBuilder.UseElasticApm(configuration,
        new AspNetCoreDiagnosticSubscriber(),
        new AspNetCoreErrorDiagnosticsSubscriber(),
        new EfCoreDiagnosticsSubscriber(),
        new HttpDiagnosticsSubscriber(),
        new SqlClientDiagnosticSubscriber(),
        new MediatrDiagnosticsSubscriber());
}