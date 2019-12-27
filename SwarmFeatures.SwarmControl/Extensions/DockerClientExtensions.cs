using System.Linq;
using Docker.DotNet;

namespace SwarmFeatures.SwarmControl.Extensions
{
    internal static class DockerClientExtensions
    {
        public static string GetLeaderAddress(this DockerClient client)
        {
            var leader = client.Swarm.ListNodesAsync().GetAwaiter().GetResult()
                .FirstOrDefault(node => node.ManagerStatus.Leader);
            return leader.Status.Addr;
        }
    }
}