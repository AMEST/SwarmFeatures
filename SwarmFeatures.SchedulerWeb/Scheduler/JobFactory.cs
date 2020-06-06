using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace SwarmFeatures.SchedulerWeb.Scheduler
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceFactory;
        
        public JobFactory(IServiceProvider serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var job = _serviceFactory.GetService(bundle.JobDetail.JobType) as IJob;
            return job;

        }

        public void ReturnJob(IJob job)
        {
            //Do something if need
        }
    }
}