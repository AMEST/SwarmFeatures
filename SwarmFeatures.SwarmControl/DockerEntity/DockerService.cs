using System.Collections.Generic;

namespace SwarmFeatures.SwarmControl.DockerEntity
{
    public class DockerService
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ulong Replicas { get; set; }

        public string Image { get; set; }

        public DockerServicePlacement Placement { get; set; }

        public Dictionary<string, string> Labels { get; set; }

        public List<PortConfiguration> Ports { get; set; }
    }
}