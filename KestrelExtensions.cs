using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

namespace WorkerWebProject
{
    public static class KestrelExtensions
    {
        public static IServiceCollection AddStandaloneKestrel(
            this IServiceCollection services,
            Action<IApplicationBuilder> app)
        {
            services.AddStandaloneKestrel(new StandaloneKestrelPipeline(app));
            return services;
        }

        public static IServiceCollection AddStandaloneKestrel<TPipeline>(this IServiceCollection services)
            where TPipeline : class, IKestrelPipeline
        {
            services.AddHostedService<Worker>();
            services.Configure<KestrelServerOptions>("Grpc",
                options => { options.ListenLocalhost(50060, o => o.Protocols = HttpProtocols.Http1); });
            services.AddSingleton<IKestrelPipeline, TPipeline>();
            return services;
        }

        public static IServiceCollection AddStandaloneKestrel<TPipeline>(this IServiceCollection services,
            TPipeline pipeline)
            where TPipeline : class, IKestrelPipeline
        {
            services.AddHostedService<Worker>();
            services.Configure<KestrelServerOptions>("Grpc",
                options => { options.ListenLocalhost(50060, o => o.Protocols = HttpProtocols.Http1); });
            services.AddSingleton<IKestrelPipeline>(pipeline);
            return services;
        }
    }
}