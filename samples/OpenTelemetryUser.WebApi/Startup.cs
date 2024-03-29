﻿using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Be.Vlaanderen.Basisregisters.Api;
using Be.Vlaanderen.Basisregisters.OpenTelemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace OpenTelemetryUser.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _environment;
        private IContainer _applicationContainer;

        public Startup(IConfiguration configuration, IHostEnvironment environment) 
        {
            _configuration = configuration;
            _environment = environment;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddOpenTelemetryTracing(typeof(Startup).Namespace, isDevelopment:_environment.IsDevelopment())
                .AddOpenTelemetryMetrics(true)
                .AddOpenTelemetryLogging()
                .ConfigureDefaultForApi<Startup>(new StartupConfigureOptions
                {
                    Cors =
                    {
                        Origins = _configuration
                            .GetSection("Cors")
                            .GetChildren()
                            .Select(c => c.Value)
                            .ToArray()!
                    },
                    Swagger =
                    {
                        ApiInfo = (_, description) => new OpenApiInfo
                        {
                            Version = description.ApiVersion.ToString(),
                            Title = "Basisregisters Vlaanderen OpenTelemetry Sample Web API"
                        },
                        XmlCommentPaths = new[] { typeof(Startup).GetTypeInfo().Assembly.GetName().Name }!
                    }
                });

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new ApiModule(services));
            _applicationContainer = containerBuilder.Build();

            return new AutofacServiceProvider(_applicationContainer);
        }

        public void Configure(
            IServiceProvider serviceProvider,
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IHostApplicationLifetime appLifetime,
            ILoggerFactory loggerFactory,
            IApiVersionDescriptionProvider apiVersionProvider)
        {
            app
                .UseDefaultForApi(new StartupUseOptions
                {
                    Common =
                    {
                        ApplicationContainer = _applicationContainer,
                        ServiceProvider = serviceProvider,
                        HostingEnvironment = env,
                        ApplicationLifetime = appLifetime,
                        LoggerFactory = loggerFactory
                    },
                    Api =
                    {
                        VersionProvider = apiVersionProvider,
                        Info = groupName => $"Basisregisters Vlaanderen - OpenTelemetry Sample Web API {groupName}",
                        CSharpClientOptions =
                        {
                            ClassName = "OpenTelemetryUser.WebApi",
                            Namespace = "Be.Vlaanderen.Basisregisters"
                        },
                        TypeScriptClientOptions =
                        {
                            ClassName = "OpenTelemetryUser.WebApi"
                        }
                    },
                    Server =
                    {
                        PoweredByName = "Vlaamse overheid - Basisregisters Vlaanderen",
                        ServerName = "Digitaal Vlaanderen"
                    }
                });
        }
    }
}
