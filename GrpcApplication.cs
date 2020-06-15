using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace WorkerWebProject
{
    public class GrpcApplication : IHttpApplication<HttpContext>
    {
        private readonly RequestDelegate _request;
        private readonly IHttpContextFactory _httpContextFactory;

        public GrpcApplication(RequestDelegate request, IServiceProvider provider)
        {
            _request = request;
            _httpContextFactory = new DefaultHttpContextFactory(provider);
        }

        public HttpContext CreateContext(IFeatureCollection contextFeatures)
        {
            return _httpContextFactory.Create(contextFeatures);
        }

        public void DisposeContext(HttpContext context, Exception exception)
        {
            _httpContextFactory.Dispose(context);
        }

        public async Task ProcessRequestAsync(HttpContext context)
        {
            await _request(context);
        }
    }
}