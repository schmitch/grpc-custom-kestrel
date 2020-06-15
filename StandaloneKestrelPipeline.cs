using System;
using Microsoft.AspNetCore.Builder;

namespace WorkerWebProject
{
    public class StandaloneKestrelPipeline : IKestrelPipeline
    {
        private readonly Action<IApplicationBuilder> _builder;

        public StandaloneKestrelPipeline(Action<IApplicationBuilder> builder)
        {
            _builder = builder;
        }

        public void Configure(IApplicationBuilder builder)
        {
            _builder(builder);
        }
    }
}