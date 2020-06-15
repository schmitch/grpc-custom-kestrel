using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WorkerWebProject
{
    public class Worker : IHostedService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly KestrelServerOptions _options;
        private readonly IServiceProvider _provider;

        private KestrelServer _server;

        public Worker(ILogger<Worker> logger, ILoggerFactory loggerFactory,
            IOptionsMonitor<KestrelServerOptions> options,
            IServiceProvider provider)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;
            _provider = provider;
            _options = options.Get("Grpc");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var transportOptions = new SocketTransportOptions
            {
            };

            _server = new KestrelServer(
                Options.Create(_options),
                new SocketTransportFactory(Options.Create(transportOptions), _loggerFactory),
                _loggerFactory);
            
            var app = new ApplicationBuilderFactory(_provider).CreateBuilder(_server.Features);
            
            app.Run(async context => { await context.Response.WriteAsync("Hello World", cancellationToken); });

            var requestDelegate = app.Build();


            await _server.StartAsync(new GrpcApplication(requestDelegate, _provider), cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _server.StopAsync(cancellationToken);
        }
    }
}