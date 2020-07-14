using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz;
using ILogger = Serilog.ILogger;

namespace SwarmFeatures.SchedulerWeb.Workers
{
    public class QuartzHostedService : IHostedService
    {
        private readonly IApplicationLifetime _applicationLifetime;
        private readonly ILogger _logger;
        private readonly IScheduler _quartzScheduler;

        public QuartzHostedService(IApplicationLifetime applicationLifetime, ILogger logger, IScheduler quartzScheduler)
        {
            _applicationLifetime = applicationLifetime;
            _logger = logger;
            _quartzScheduler = quartzScheduler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _applicationLifetime.ApplicationStarted.Register(OnStart);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStart()
        {
            _logger.Information("Quartz Started");
            _quartzScheduler.Start();
        }
    }
}