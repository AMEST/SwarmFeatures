using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SwarmFeatures.SwarmAutoProxy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : Controller
    {
        private readonly IProxyHostResolver _resolver;

        public ServiceController(IProxyHostResolver resolver)
        {
            _resolver = resolver;
        }

        [HttpGet]
        public async Task<IActionResult> AvailableProxyEndpoints()
        {
            var hosts = await _resolver.GetAll();

            return Ok(hosts);
        }
    }
}