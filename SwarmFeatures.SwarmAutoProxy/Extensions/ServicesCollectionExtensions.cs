using Microsoft.Extensions.DependencyInjection;

namespace SwarmFeatures.SwarmAutoProxy.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddAutoProxyModule(this IServiceCollection services)
        {
            services.AddSingleton<IProxyHostResolver, SwarmProxyHostResolver>();

            return services;
        }
    }
}