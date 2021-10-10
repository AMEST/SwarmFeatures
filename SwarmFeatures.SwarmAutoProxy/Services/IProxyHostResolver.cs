using System.Collections.Generic;
using System.Threading.Tasks;
using SwarmFeatures.SwarmAutoProxy.Data;

namespace SwarmFeatures.SwarmAutoProxy.Services
{
    public interface IProxyHostResolver
    {
        Task<ProxyHost> Resolve(string host);

        Task<List<ProxyHost>> GetAll();
    }
}