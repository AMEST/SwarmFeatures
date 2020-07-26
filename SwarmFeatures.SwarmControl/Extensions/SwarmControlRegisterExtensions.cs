using Microsoft.Extensions.DependencyInjection;
using SwarmFeatures.SwarmControl.Configuration;

namespace SwarmFeatures.SwarmControl.Extensions
{
    public static class SwarmControlRegisterExtensions
    {
        public static IServiceCollection AddSwarmControlModule(this IServiceCollection services,
            ISwarmControlConfiguration configuration)
        {
            services.AddSingleton<ISwarmManager, SwarmManager>()
                .AddSingleton<IDockerClientFactory, DockerClientFactory>()
                .AddSingleton<ISwarmControlConfiguration>(configuration);
            return services;
        }
    }
}