using System;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;

namespace SwarmFeatures.SwarmControl.Extensions
{
    internal static class DockerClientExtensions
    {
        private static bool IsManagerFlag = true;
        private static DateTime ManagerCheckTimestamp = DateTime.MinValue;

        public static async Task<string> GetLeaderAddress(this DockerClient client)
        {
            var leader = (await client.Swarm.ListNodesAsync())
                .FirstOrDefault(node => node.ManagerStatus.Leader);
            return leader?.Status.Addr;
        }

        public static async Task<bool> IsManager(this DockerClient client){
            if(ManagerCheckTimestamp.AddMinutes(5) > DateTime.UtcNow)
                return IsManagerFlag;
            
            try{
                _ = await client.Swarm.ListNodesAsync();
                IsManagerFlag = true;
            }catch(Exception){
                IsManagerFlag = false;
            }
            ManagerCheckTimestamp = DateTime.UtcNow;
            return IsManagerFlag;
        }
    }
}