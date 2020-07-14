using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using SwarmFeatures.SchedulerWeb.Scheduler;
using SwarmFeatures.SwarmControl;

namespace SwarmFeatures.SchedulerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController: Controller
    {
        private readonly ISchedulerManager _manager;
        private readonly ISwarmManager _swarmManager;

        public SchedulerController(ISchedulerManager manager, ISwarmManager _swarmManager)
        {
            _manager = manager;
            this._swarmManager = _swarmManager;
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _manager.GetScheduledServices());
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Run([FromRoute] string id)
        {
            await _manager.RunService(id);
            return Ok();
        }

        [HttpGet("[action]")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Nodes()
        {
            var nodes = await _swarmManager.GetNodes();
            return Ok(nodes);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> AddTask([FromRoute] string id, [FromQuery] string cron)
        {
            await _manager.AddQuartzTask(id, cron);
            return Ok();
        }

        [HttpGet("[action]")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Tasks()
        {
            var tasksList = await _manager.ListQuartzTasks();
            return Ok(tasksList);
        }
    }
}