using System;
using System.Text.RegularExpressions;
using Docker.DotNet;
using SwarmFeatures.SwarmControl.Configuration;
using SwarmFeatures.SwarmControl.Extensions;

namespace SwarmFeatures.SwarmControl
{
    internal class DockerClientFactory : IDockerClientFactory
    {
        private readonly ISwarmControlConfiguration _configuration;

        public DockerClientFactory(ISwarmControlConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DockerClient CreateDockerClient()
        {
            var dockerClient = new DockerClientConfiguration(new Uri(_configuration.DockerUri)).CreateClient();

            if (!_configuration.AutoSwitchConnectionToLeader) return dockerClient;

            var leaderAddress = dockerClient.GetLeaderAddress();
            var leaderUri = new Uri(Regex.Replace(_configuration.DockerUri, "\\/\\/.*:", $"//{leaderAddress}:"));
            dockerClient = new DockerClientConfiguration(leaderUri).CreateClient();

            return dockerClient;
        }
    }
}