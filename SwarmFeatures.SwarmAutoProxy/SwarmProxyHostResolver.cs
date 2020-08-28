using SwarmFeatures.SwarmAutoProxy.Data;
using SwarmFeatures.SwarmControl;
using SwarmFeatures.SwarmControl.DockerEntity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;

namespace SwarmFeatures.SwarmAutoProxy
{
    public class SwarmProxyHostResolver : IProxyHostResolver
    {
        private readonly ISwarmManager _manager;

        private readonly ILogger _logger;

        private ConcurrentBag<ProxyHost> _proxyHostsCache;

        private DockerNode _cacheNode = new DockerNode();

        private DateTimeOffset _cacheTime;

        public SwarmProxyHostResolver(ISwarmManager manager, ILogger logger)
        {
            _manager = manager;
            _logger = logger.ForContext<SwarmProxyHostResolver>();
            _cacheTime = DateTimeOffset.Now;
        }

        public async Task<ProxyHost> Resolve(string host)
        {
            var services = await GetAllProxyHost();

            return services.FirstOrDefault(s => s.Hostname.Equals(host, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<List<ProxyHost>> GetAll()
        {
            var services = await GetAllProxyHost();

            return services;
        }

        private async Task<List<ProxyHost>> GetAllProxyHost()
        {
            if (_proxyHostsCache == null || _proxyHostsCache != null && _proxyHostsCache.IsEmpty ||
                _cacheTime.AddSeconds(10).ToUnixTimeSeconds() < DateTimeOffset.Now.ToUnixTimeSeconds())
            {
                var allServices = await _manager.GetDockerServices();

                var hosts = await _manager.GetNodes();

                lock (_cacheNode)
                {
                    _cacheNode = hosts.OrderBy(a => Guid.NewGuid()).FirstOrDefault(host =>
                        host.Availability.Equals("active", StringComparison.OrdinalIgnoreCase));
                }

                _proxyHostsCache = new ConcurrentBag<ProxyHost>(allServices.Where(service =>
                        service.Labels.ContainsKey(ProxyLabels.Enable)
                        && service.Labels.ContainsKey(ProxyLabels.Hostname)
                        && bool.Parse(service.Labels[ProxyLabels.Enable]))
                    .Select(service => CreateHost(service, _cacheNode)).ToList());

                _cacheTime = DateTimeOffset.Now;
                _logger.Debug("Proxied hosts list updated!");
            }

            return _proxyHostsCache.ToList();
        }

        private ProxyHost CreateHost(DockerService service, DockerNode node)
        {
            if (service.Labels.ContainsKey(ProxyLabels.Address))
                return new ProxyHost
                {
                    Address = service.Labels[ProxyLabels.Address],
                    Hostname = service.Labels[ProxyLabels.Hostname],
                    ServiceName = service.Name
                };

            if (!service.Ports.Any())
                return new ProxyHost();

            var port = service.Ports[0];
            var address = $"{node.Address}:{port.PublishedPort}";

            return new ProxyHost
            {
                Address = address,
                Hostname = service.Labels[ProxyLabels.Hostname],
                ServiceName = service.Name
            };
        }
    }
}