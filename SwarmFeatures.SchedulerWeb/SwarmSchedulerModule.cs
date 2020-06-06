using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using SwarmFeatures.SchedulerWeb.Scheduler;
using SwarmFeatures.SchedulerWeb.Workers;

namespace SwarmFeatures.SchedulerWeb
{
    public static class SwarmSchedulerModule
    {
        public static IServiceCollection AddSwarmSchedulerModule(this IServiceCollection services)
        {
            services.AddHostedService<RunnableHostedService>();
            services.AddHostedService<DockerJobHostedService>();
            services.AddSingleton<ISchedulerManager, SchedulerManager>();
            services.AddSingleton<IJobFactory,JobFactory>();
            services.AddTransient<SwarmExecuteJob>();
            services.AddSingleton(CreateScheduler);

            return services;
        }

        private static IScheduler CreateScheduler(IServiceProvider provider)
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();
            scheduler.JobFactory = provider.GetService<IJobFactory>();
            return scheduler;
        }
    }
}