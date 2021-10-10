// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace SwarmFeatures.SwarmAutoProxy.Middlewares.Proxy
{
    public static class ProxyServiceCollectionExtensions
    {
        public static IServiceCollection AddProxy(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            return services.AddScoped<ProxyService>();
        }

        public static IServiceCollection AddProxy(this IServiceCollection services,
            Action<SharedProxyOptions> configureOptions)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

            services.Configure(configureOptions);
            return services.AddScoped<ProxyService>();
        }
    }
}