// <copyright file="FakeLocationCacheFactory.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests.Fakes
{
    using System;
    using GeneGenie.Sawmill.Caching;
    using GeneGenie.Sawmill.Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    public class FakeLocationCacheFactory : ILocationCacheFactory
    {
        private readonly IServiceProvider serviceProvider;

        public FakeLocationCacheFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public string DiskStoragePath { get; private set; }

        public FakeLogger<LocationCacheMemory> L1Logger { get; private set; }

        public FakeLogger<LocationCacheDisk> L2Logger { get; private set; }

        public LocationCacheMemory Level1Cache { get; private set; }

        public LocationCacheDisk Level2Cache { get; private set; }

        public LocationCreator LocationCreator { get; private set; }

        public FakeLogger<CacheManager> Logger { get; private set; }

        /// <summary>
        /// Creates an instance of the LocationCache that has a fake logger attached for
        /// checking that code is called.
        /// Suitable for unit and integration testing.
        /// </summary>
        /// <returns></returns>
        public CacheManager Create()
        {
            DiskStoragePath = Guid.NewGuid().ToString() + ".json";
            L1Logger = serviceProvider.GetRequiredService<FakeLogger<LocationCacheMemory>>();
            L2Logger = serviceProvider.GetRequiredService<FakeLogger<LocationCacheDisk>>();
            Level1Cache = new LocationCacheMemory(L1Logger);
            Level2Cache = new LocationCacheDisk(L2Logger, DiskStoragePath);
            LocationCreator = serviceProvider.GetRequiredService<LocationCreator>();
            Logger = serviceProvider.GetRequiredService<FakeLogger<CacheManager>>();

            return new CacheManager(Level1Cache, Level2Cache, LocationCreator, Logger);
        }
    }
}
