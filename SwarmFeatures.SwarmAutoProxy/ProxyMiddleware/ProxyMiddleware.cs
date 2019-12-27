// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Serilog;
using SwarmFeatures.SwarmAutoProxy.Extensions;
using System;
using System.Threading.Tasks;

namespace SwarmFeatures.SwarmAutoProxy.ProxyMiddleware
{
    /// <summary>
    /// Proxy Middleware
    /// </summary>
    public class ProxyMiddleware
    {
        private const int DefaultWebSocketBufferSize = 4096;

        private readonly RequestDelegate _next;
        private readonly IProxyHostResolver _hostResolver;
        private readonly ILogger _logger;
        private readonly ProxyOptions _options;

        private static readonly string[] NotForwardedWebSocketHeaders = new[]
            {"Connection", "Host", "Upgrade", "Sec-WebSocket-Key", "Sec-WebSocket-Version"};

        public ProxyMiddleware(RequestDelegate next, IProxyHostResolver hostResolver, ILogger logger,
            IOptions<ProxyOptions> options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next;
            _hostResolver = hostResolver;
            _logger = logger.ForContext<ProxyMiddleware>();
            _options = options.Value;
        }

        public Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Uri uri;
            if (!_options.Host.HasValue)
            {
                var resolvedHost = _hostResolver.Resolve(context.Request.Host.Value.IgnorePort()).GetAwaiter()
                    .GetResult();

                if (resolvedHost == null)
                    return _next(context);

                var scheme = "http";
                if (context.WebSockets.IsWebSocketRequest)
                    scheme = context.Request.Scheme;

                uri = new Uri(UriHelper.BuildAbsolute(scheme, new HostString(resolvedHost.Address),
                    path: context.Request.Path,
                    query: context.Request.QueryString.Add(_options.AppendQuery)));

                _logger.Debug(
                    $"Proxy executed for service {resolvedHost.ServiceName} to {uri.AbsoluteUri} by {resolvedHost.Hostname}");
            }
            else
            {
                uri = new Uri(UriHelper.BuildAbsolute(_options.Scheme, _options.Host, _options.PathBase,
                    context.Request.Path, context.Request.QueryString.Add(_options.AppendQuery)));
            }

            return context.ProxyRequest(uri);
        }
    }
}