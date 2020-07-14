using Microsoft.Extensions.Hosting;
using Serilog;
using SwarmFeatures.SchedulerWeb.Scheduler;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SwarmFeatures.SchedulerWeb.Workers
{
    public class DockerJobHostedService : IHostedService
    {
        private readonly ISchedulerManager _schedulerManager;
        private readonly ILogger _logger;
        private Timer _swarmTimer;

        public DockerJobHostedService(ISchedulerManager schedulerManager, ILogger logger)
        {
            _schedulerManager = schedulerManager;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _swarmTimer = new Timer(Tick, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _swarmTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async void Tick(object state)
        {
            var dockerServices = await _schedulerManager.GetScheduledServices();
            var quartzJobs = await _schedulerManager.ListQuartzTasks();
            foreach (var service in dockerServices)
            {
                if (quartzJobs.Any(job => service.Id == job.Id))
                {
                    _logger.Debug($"Service {service.Name} already in jobs list");
                    continue;
                }

                await _schedulerManager.AddQuartzTask(service.Id, service.GetServiceCron());
                _logger.Information($"Service {service.Name} added to cron with {service.GetServiceCron()}");
            }

            foreach (var job in quartzJobs.Where(job => !dockerServices.Any(service => service.Id == job.Id)))
            {
                _logger.Information("Job {JobId} not associated with Service. Deleting job", job.Id);
                await _schedulerManager.RemoveQuartzTask(job.Id);
            }
        }
    }
}