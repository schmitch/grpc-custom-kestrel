using Microsoft.AspNetCore.Builder;

namespace WorkerWebProject
{
    public interface IKestrelPipeline
    {
        void Configure(IApplicationBuilder builder);
    }
}