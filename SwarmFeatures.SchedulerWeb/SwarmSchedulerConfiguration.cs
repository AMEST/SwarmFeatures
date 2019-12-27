using Microsoft.Extensions.Configuration;
using SwarmFeatures.SwarmControl.Configuration;

namespace SwarmFeatures.SchedulerWeb
{
    public class SwarmSchedulerConfiguration : ISwarmControlConfiguration
    {
        private readonly IConfiguration _configuration;

        public SwarmSchedulerConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string DockerUri => GetValue("DockerClient:Uri");
        public bool AutoSwitchConnectionToLeader => false;

        private string GetValue(string key)
        {
            return _configuration[key];
        }

        private T GetValue<T>(string key)
        {
            return _configuration.GetValue<T>(key);
        }
    }
}