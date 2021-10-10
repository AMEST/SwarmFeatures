using SwarmFeatures.SwarmAutoProxy.Data;
using SwarmFeatures.SwarmControl;
using SwarmFeatures.SwarmControl.DockerEntity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;

namespace SwarmFeatures.SwarmAutoProxy.Services
{
    public class SwarmProxyHostResolver : IProxyHostResolver
    {
        private readonly ISwarmManager _manager;
        private readonly ILogger _logger;
        private ConcurrentBag<ProxyHost> _proxyHostsCache = new ConcurrentBag<ProxyHost>();
        private DockerNode _cacheNode = new DockerNode();
        private DateTimeOffset _cacheTime;
        private bool _cacheUpdateInProgress;

        public SwarmProxyHostResolver(ISwarmManager manager, ILogger logger)
        {
            _manager = manager;
            _logger = logger.ForContext<SwarmProxyHostResolver>();
            _cacheTime = DateTimeOffset.MinValue;
        }

        public async Task<ProxyHost> Resolve(string host)
        {
            await Update();
            return _proxyHostsCache.FirstOrDefault(s => s.Hostname.Equals(host, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<List<ProxyHost>> GetAll()
        {
            await Update();
            return _proxyHostsCache.ToList();
        }

        private async Task Update()
        {
            if (_cacheUpdateInProgress)
                return;
            if (_cacheTime.AddSeconds(10).ToUnixTimeSeconds() >= DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                return;
            try
            {
                _cacheUpdateInProgress = true;
                var allServices = await _manager.GetDockerServices();

                var hosts = await _manager.GetNodes();

                lock (_cacheNode)
                {
                    _cacheNode = hosts.OrderBy(a => Guid.NewGuid()).FirstOrDefault(host =>
                        host.Availability.Equals("active", StringComparison.OrdinalIgnoreCase));
                }
                var proxiedServices = allServices.Where(service =>
                        service.Labels.ContainsKey(ProxyLabels.Enable)
                        && service.Labels.ContainsKey(ProxyLabels.Hostname)
                        && bool.Parse(service.Labels[ProxyLabels.Enable])).ToArray();
                _proxyHostsCache.Clear();
                foreach (var service in proxiedServices)
                {
                    var host = await CreateHost(service, _cacheNode);
                    _proxyHostsCache.Add(host);
                }

                _cacheTime = DateTimeOffset.UtcNow;
                _logger.Debug("Proxied hosts list updated!");
            }
            finally
            {
                _cacheUpdateInProgress = false;
            }
        }

        private async Task<ProxyHost> CreateHost(DockerService service, DockerNode node)
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

            var randomTask = service.Tasks?
                .Where(t => t.State == DockerTaskState.Running)
                .OrderBy(t => Guid.NewGuid())
                .FirstOrDefault();

            var port = service.Ports.First();

            if (randomTask == null)
                return new ProxyHost
                {
                    Address = $"{node.Address}:{port.PublishedPort}",
                    Hostname = service.Labels[ProxyLabels.Hostname],
                    ServiceName = service.Name
                };


            var taskNode = await _manager.GetNodeById(randomTask.NodeID);
            if (taskNode != null)
                return new ProxyHost
                {
                    Address = $"{taskNode.Address}:{port.PublishedPort}",
                    Hostname = service.Labels[ProxyLabels.Hostname],
                    ServiceName = service.Name
                };

            return new ProxyHost
            {
                Address = $"{node.Address}:{port.PublishedPort}",
                Hostname = service.Labels[ProxyLabels.Hostname],
                ServiceName = service.Name
            };
        }
    }
}