// <copyright file="LocationCacheFactory.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill
{
    using System;
    using GeneGenie.Sawmill.Caching;
    using GeneGenie.Sawmill.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class LocationCacheFactory : ILocationCacheFactory
    {
        private readonly IServiceProvider serviceProvider;

        public LocationCacheFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public CacheManager Create()
        {
            var level1Cache = serviceProvider.GetRequiredService<LocationCacheMemory>();
            var level2Cache = serviceProvider.GetRequiredService<LocationCacheDisk>();
            var locationCreator = serviceProvider.GetRequiredService<LocationCreator>();
            var logger = serviceProvider.GetRequiredService<ILogger<CacheManager>>();

            return new CacheManager(level1Cache, level2Cache, locationCreator, logger);
        }
    }
}
