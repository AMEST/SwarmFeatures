using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl.Matchers;
using SwarmFeatures.SwarmControl;
using SwarmFeatures.SwarmControl.DockerEntity;

namespace SwarmFeatures.SchedulerWeb.Scheduler
{
    public class SchedulerManager : ISchedulerManager
    {
        private readonly ISwarmManager _manager;
        private readonly IScheduler _scheduler;

        public SchedulerManager(ISwarmManager manager, IScheduler scheduler)
        {
            _manager = manager;
            _scheduler = scheduler;
        }

        /// <inheritdoc />
        public async Task RunService(string id)
        {
            await StopService(id);

            var service = await GetScheduledServiceById(id);

            if (service == null)
                return;

            service.Replicas = (ulong) service.GetScheduleReplicas();
            service.SetTimestamp();

            await _manager.UpdateService(service);

            await Task.Delay(500);
        }

        /// <inheritdoc />
        public async Task StopService(string id)
        {
            var service = await GetScheduledServiceById(id);

            if (service == null)
                return;

            service.Replicas = 0;

            await _manager.UpdateService(service);

            await Task.Delay(1000);
        }

        /// <inheritdoc />
        public async Task<List<DockerService>> GetScheduledServices()
        {
            var services = await _manager.GetDockerServices();
            return services?.Where(service => service.Labels.Any(label => label.Key.Equals(SchedulerLabels.Enable)))
                .ToList();
        }

        /// <inheritdoc />
        public async Task<DockerService> GetScheduledServiceById(string id)
        {
            var service = await _manager.GetServiceById(id);
            return service != null
                   && service.Labels.Any(label => label.Key.Equals(SchedulerLabels.Enable))
                ? service
                : null;
        }

        /// <inheritdoc />
        public async Task AddQuartzTask(string id, string cron = "0 * * * * ? *")
        {
            var service = await GetScheduledServiceById(id);
            var jobDetail = JobBuilder.Create<SwarmExecuteJob>()
                .WithIdentity(id)
                .WithDescription(service.Name)
                .Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity(id)
                .StartNow()
                .WithCronSchedule(cron)
                .Build();
            await _scheduler.ScheduleJob(jobDetail, trigger);
        }

        /// <inheritdoc />
        public async Task RemoveQuartzTask(string id)
        {
            foreach (var job in await _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()))
            {
                if (!job.Name.Equals(id, StringComparison.OrdinalIgnoreCase)) continue;
                await _scheduler.DeleteJob(job);
                return;
            }
        }

        /// <inheritdoc />
        public async Task<List<DockerService>> ListQuartzTasks()
        {
            var result = new List<DockerService>();
            foreach (var jobKey in await _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()))
            {
                var service = await GetScheduledServiceById(jobKey.Name);
                result.Add(service ?? new DockerService {Id = jobKey.Name});
            }

            return result;
        }
    }
}