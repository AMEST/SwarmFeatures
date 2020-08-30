using System;
using System.Collections.Generic;

namespace SwarmFeatures.SwarmControl.DockerEntity
{
    public class DockerTask
    {
        public string Id { get; set; }

        public string ServiceId { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public IDictionary<string, string> Labels { get; set; }

        public long Slot { get; set; }

        public DockerTaskState State { get; set; }

        public string NodeID { get; set; }
    }
}