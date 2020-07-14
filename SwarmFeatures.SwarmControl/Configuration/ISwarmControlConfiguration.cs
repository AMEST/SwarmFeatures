namespace SwarmFeatures.SwarmControl.Configuration
{
    public interface ISwarmControlConfiguration
    {
        string DockerUri { get; }

        bool AutoSwitchConnectionToLeader { get; }
    }
}