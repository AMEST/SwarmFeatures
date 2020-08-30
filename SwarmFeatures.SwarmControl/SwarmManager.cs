using Docker.DotNet;
using Docker.DotNet.Models;
using SwarmFeatures.SwarmControl.DockerEntity;
using SwarmFeatures.SwarmControl.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwarmFeatures.SwarmControl
{
    internal class SwarmManager : ISwarmManager
    {
        private readonly Lazy<DockerClient> _dockerClient;

        public SwarmManager(IDockerClientFactory dockerClientFactory)
        {
            _dockerClient = new Lazy<DockerClient>(dockerClientFactory.CreateDockerClient);
        }

        public async Task RemoveService(DockerService dockerService)
        {
            await _dockerClient.Value.Swarm.RemoveServiceAsync(dockerService.Id);
        }

        public async Task UpdateService(DockerService dockerService)
        {
            var service = await GetService(dockerService);

            if (service == null)
                return;

            var serviceUpdateParams = new ServiceUpdateParameters
            {
                Service = service.Spec,
                Version = Convert.ToInt64(service.Version.Index)
            };

            serviceUpdateParams.Service.Labels = dockerService.Labels;
            serviceUpdateParams.Service.Mode.Replicated.Replicas = dockerService.Replicas;
            serviceUpdateParams.Service.TaskTemplate.ContainerSpec.Image = dockerService.Image;
            serviceUpdateParams.Service.TaskTemplate.Placement = dockerService.Placement.ToObject();
            serviceUpdateParams.Service.TaskTemplate.ForceUpdate++;

            await _dockerClient.Value.Swarm.UpdateServiceAsync(dockerService.Id, serviceUpdateParams);
        }

        public async Task<List<DockerService>> GetDockerServices()
        {
            var services = await _dockerClient.Value.Swarm.ListServicesAsync();
            var servicesEntity = services?.ToEntity();

            if (servicesEntity == null)
                return null;

            foreach (var service in servicesEntity)
            {
                service.Tasks = await GetServiceTask(service.Id);
            }

            return servicesEntity;
        }

        public async Task<DockerService> GetServiceById(string id)
        {
            var service = (await GetService(new DockerService {Id = id})).ToEntity();
            if (service != null)
                service.Tasks = await GetServiceTask(service.Id);
            return service;
        }

        public async Task<IEnumerable<DockerNode>> GetNodes()
        {
            var nodes = await _dockerClient.Value.Swarm.ListNodesAsync();
            return nodes?.ToEntity();
        }

        public async Task<DockerNode> GetNodeById(string id)
        {
            var node = (await GetNodes())?.SingleOrDefault(n =>
                n.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            return node;
        }

        private async Task<SwarmService> GetService(DockerService dockerService)
        {
            return (await _dockerClient.Value.Swarm.ListServicesAsync())
                ?.FirstOrDefault(s => s.ID.Equals(dockerService.Id));
        }

        private async Task<List<DockerTask>> GetServiceTask(string serviceId)
        {
            var tasks = (await _dockerClient.Value.Tasks.ListAsync())?.Where(task =>
                task.ServiceID.Equals(serviceId, StringComparison.OrdinalIgnoreCase));
            return tasks?.ToEntity();
        }
    }
}