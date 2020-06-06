using System;
using System.Linq;
using SwarmFeatures.SwarmControl.DockerEntity;

namespace SwarmFeatures.SchedulerWeb.Scheduler
{
    public static class DockerServiceExtensions
    {
        public static void SetTimestamp(this DockerService service)
        {
            var currentTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            if (service.Labels.Any(l => l.Key.Equals(SchedulerLabels.LastSchedule)))
                service.Labels[SchedulerLabels.LastSchedule] = currentTimeStamp;
            else
                service.Labels.Add(SchedulerLabels.LastSchedule, currentTimeStamp);
        }

        public static string GetServiceCron(this DockerService service)
        {
            return service.Labels[SchedulerLabels.Schedule];
        }

        public static string GetScheduleEnabled(this DockerService service)
        {
            return service.Labels[SchedulerLabels.Enable];
        }
    }
}