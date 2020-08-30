namespace SwarmFeatures.SwarmControl.DockerEntity
{
    public enum DockerTaskState
    {
        New,
        Allocated,
        Pending,
        Assigned,
        Accepted,
        Preparing,
        Ready,
        Starting,
        Running,
        Complete,
        Shutdown,
        Failed,
        Rejected,
        Remove,
        Orphaned,
    }
}