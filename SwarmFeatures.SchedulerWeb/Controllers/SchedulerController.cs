using Microsoft.AspNetCore.Mvc;
using SwarmFeatures.SchedulerWeb.Scheduler;
using System.Threading.Tasks;

namespace SwarmFeatures.SchedulerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : Controller
    {
        private readonly ISchedulerManager _manager;

        public SchedulerController(ISchedulerManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Get all services with label sf.scheduler.enable = true
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _manager.GetScheduledServices());
        }

        /// <summary>
        /// Run service by id
        /// </summary>
        /// <param name="id">docker service id</param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Run([FromRoute] string id)
        {
            await _manager.RunService(id);
            return Ok();
        }

        /// <summary>
        /// Add quartz task
        /// </summary>
        /// <param name="id">docker service id</param>
        /// <param name="cron">cron rule</param>
        /// <returns></returns>
        [HttpPost("task/{id}")]
        public async Task<IActionResult> AddTask([FromRoute] string id, [FromQuery] string cron)
        {
            await _manager.AddQuartzTask(id, cron);
            return Ok();
        }

        /// <summary>
        /// Add quartz task
        /// </summary>
        /// <param name="id">docker service id</param>
        /// <param name="cron">cron rule</param>
        /// <returns></returns>
        [HttpDelete("task/{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] string id)
        {
            await _manager.RemoveQuartzTask(id);
            return Ok();
        }

        /// <summary>
        /// List all quartz tasks
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Tasks()
        {
            var tasksList = await _manager.ListQuartzTasks();
            return Ok(tasksList);
        }
    }
}