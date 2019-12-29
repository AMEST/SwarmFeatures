﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SwarmFeatures.SwarmControl.DockerEntity;

namespace SwarmFeatures.SchedulerWeb.Scheduler
{
    public interface ISchedulerManager
    {
        Task RunScheduledService(string id);
        Task StopScheduledService(string id);
        Task<List<DockerService>> GetScheduledDockerServices();
        Task<DockerService> GetScheduledDockerServiceById(string id);
    }
}