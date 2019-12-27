namespace SwarmFeatures.SwarmAutoProxy.Extensions
{
    public static class ProxyExtensions
    {
        public static string IgnorePort(this string address)
        {
            return address.Split(":")[0];
        }
    }
}