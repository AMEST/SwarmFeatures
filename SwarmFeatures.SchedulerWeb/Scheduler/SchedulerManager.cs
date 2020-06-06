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

        public async Task RunScheduledService(string id)
        {
            await StopScheduledService(id);

            var service = await GetScheduledDockerServiceById(id);

            service.Replicas = 1;
            service.SetTimestamp();

            await _manager.UpdateService(service);

            await Task.Delay(500);
        }

        public async Task StopScheduledService(string id)
        {
            var service = await GetScheduledDockerServiceById(id);

            service.Replicas = 0;

            await _manager.UpdateService(service);

            await Task.Delay(1000);
        }

        public async Task<List<DockerService>> GetScheduledDockerServices()
        {
            var services = await _manager.GetDockerServices();
            return services.Where(service => service.Labels.Any(label => label.Key.Equals(SchedulerLabels.Enable)))
                .ToList();
        }

        public async Task<DockerService> GetScheduledDockerServiceById(string id)
        {
            var service = await _manager.GetServiceById(id);
            return service.Labels.Any(label => label.Key.Equals(SchedulerLabels.Enable)) ? service : null;
        }

        public async Task AddQuartzTask(string id, string cron = "0 * * * * ? *")
        {
            var service = await GetScheduledDockerServiceById(id);
            IJobDetail jobDetail = JobBuilder.Create<SwarmExecuteJob>()
                .WithIdentity(id)
                .WithDescription(service.Name)
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("MailingTrigger", "default")
                .StartNow()
                .WithCronSchedule(cron)
                .Build();
            await _scheduler.ScheduleJob(jobDetail, trigger);
        }

        public async Task RemoveQuartzTask(string id)
        {
            foreach (var job in await _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()))
            {
                if (!job.Name.Equals(id, StringComparison.OrdinalIgnoreCase)) continue;
                await _scheduler.DeleteJob(job);
                return;
            }
        }

        public async Task<List<DockerService>> ListQuartzTasks()
        {
            var result = new List<DockerService>();
            foreach (var jobKey in await _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()))
            {
                var service = await GetScheduledDockerServiceById(jobKey.Name);
                result.Add(service);
            }

            return result;
        }
    }
}