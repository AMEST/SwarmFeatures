// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace SwarmFeatures.SwarmAutoProxy.ProxyMiddleware
{
    public class ProxyService : IDisposable
    {
        private HttpClient _httpClient;
        public ProxyService(IOptions<SharedProxyOptions> options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            Options = options.Value;
        }

        public HttpClient GetOrCreate(Action<HttpClientHandler> configure)
        {
            if (_httpClient != null)
                return _httpClient;
            var handler = new HttpClientHandler();
            configure?.Invoke(handler);
            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };
            return _httpClient;
        }

        public SharedProxyOptions Options { get; private set; }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}