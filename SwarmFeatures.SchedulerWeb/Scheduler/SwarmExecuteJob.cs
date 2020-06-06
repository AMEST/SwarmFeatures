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
            await _schedulerManager.StopScheduledService(context.JobDetail.Key.Name);
            await _schedulerManager.RunScheduledService(context.JobDetail.Key.Name);
            _logger.Debug($"Job key = {context.JobDetail.Key}; description = {context.JobDetail.Description}");
        }
    }
}