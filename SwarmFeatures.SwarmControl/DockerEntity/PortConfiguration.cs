namespace SwarmFeatures.SwarmControl.DockerEntity
{
    public class PortConfiguration
    {
        public string Name { get; set; }

        public string Protocol { get; set; }

        public uint TargetPort { get; set; }

        public uint PublishedPort { get; set; }

        public string PublishMode { get; set; }
    }
}