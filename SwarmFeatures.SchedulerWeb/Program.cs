using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SwarmFeatures.SchedulerWeb
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            return CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog((ctx, config) => { config.ReadFrom.Configuration(ctx.Configuration); });
                });
    }
}