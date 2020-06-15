using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;

namespace WorkerWebProject
{
    public class GrpcApplication : IHttpApplication<HttpContext>
    {
        private readonly RequestDelegate _request;
        private readonly IServiceProvider _provider;

        public GrpcApplication(RequestDelegate request, IServiceProvider provider)
        {
            _request = request;
            _provider = provider;
        }

        public HttpContext CreateContext(IFeatureCollection contextFeatures)
        {
            var scopeFactory = _provider.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IHttpContextFactory>().Create(contextFeatures);
        }

        public void DisposeContext(HttpContext context, Exception exception)
        {
            var scopeFactory = _provider.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            scope.ServiceProvider.GetRequiredService<IHttpContextFactory>().Dispose(context);
        }

        public async Task ProcessRequestAsync(HttpContext context)
        {
            await _request(context);
        }
    }
}