using Quartz;
using System.Threading.Tasks;
using Serilog;


namespace SwarmFeatures.SchedulerWeb.Scheduler
{
    public class SwarmExecuteJob : IJob
    {
        private readonly ILogger _logger;
        private readonly ISchedulerManager _schedulerManager;

        public SwarmExecuteJob(ILogger logger, ISchedulerManager schedulerManager)
        {
            _logger = logger;
            _schedulerManager = schedulerManager;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _schedulerManager.StopService(context.JobDetail.Key.Name);
            await _schedulerManager.RunService(context.JobDetail.Key.Name);
            _logger.Debug("Container {ContainerID} running by quartz", context.JobDetail.Description);
        }
    }
}