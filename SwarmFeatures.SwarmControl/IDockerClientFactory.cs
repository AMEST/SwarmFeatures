using Docker.DotNet;

namespace SwarmFeatures.SwarmControl
{
    internal interface IDockerClientFactory
    {
        DockerClient CreateDockerClient();
    }
}