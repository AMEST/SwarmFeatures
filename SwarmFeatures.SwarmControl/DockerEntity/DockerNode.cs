using System.Collections.Generic;

namespace SwarmFeatures.SwarmControl.DockerEntity
{
    public class DockerNode
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IDictionary<string, string> Labels { get; set; }

        public string Role { get; set; }

        public string Availability { get; set; }

        public string Hostname { get; set; }

        public string Address { get; set; }
    }
}