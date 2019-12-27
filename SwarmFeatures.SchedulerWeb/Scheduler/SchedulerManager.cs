using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SwarmFeatures.SwarmControl;
using SwarmFeatures.SwarmControl.DockerEntity;

namespace SwarmFeatures.SchedulerWeb.Scheduler
{
    public class SchedulerManager : ISchedulerManager
    {
        private readonly ISwarmManager _manager;

        public SchedulerManager(ISwarmManager manager)
        {
            _manager = manager;
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
    }
}