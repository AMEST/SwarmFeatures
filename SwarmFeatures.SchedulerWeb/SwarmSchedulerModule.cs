using Microsoft.Extensions.DependencyInjection;
using SwarmFeatures.SchedulerWeb.Scheduler;

namespace SwarmFeatures.SchedulerWeb
{
    public static class SwarmSchedulerModule
    {
        public static IServiceCollection AddSwarmSchedulerModule(this IServiceCollection services)
        {
            services.AddSingleton<ISchedulerManager, SchedulerManager>();

            return services;
        }
    }
}