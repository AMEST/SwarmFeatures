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
            return services.ToEntity();
        }

        public async Task<DockerService> GetServiceById(string id)
        {
            return (await GetService(new DockerService {Id = id})).ToEntity();
        }

        public async Task<IEnumerable<DockerNode>> GetNodes()
        {
            var nodes = await _dockerClient.Value.Swarm.ListNodesAsync();
            return nodes.ToEntity();
        }

        private async Task<SwarmService> GetService(DockerService dockerService)
        {
            return (await _dockerClient.Value.Swarm.ListServicesAsync())
                .FirstOrDefault(s => s.ID.Equals(dockerService.Id));
        }
    }
}