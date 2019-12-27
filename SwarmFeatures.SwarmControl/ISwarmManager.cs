using System.Collections.Generic;
using System.Threading.Tasks;
using SwarmFeatures.SwarmControl.DockerEntity;

namespace SwarmFeatures.SwarmControl
{
    public interface ISwarmManager
    {
        Task RemoveService(DockerService dockerService);

        Task UpdateService(DockerService dockerService);

        Task<List<DockerService>> GetDockerServices();

        Task<DockerService> GetServiceById(string id);

        Task<IEnumerable<DockerNode>> GetNodes();

    }
}