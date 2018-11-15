// <copyright file="ManagerLoggingTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests.CacheTests
{
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Caching;
    using GeneGenie.Sawmill.Models;
    using GeneGenie.Sawmill.Tests.Fakes;
    using GeneGenie.Sawmill.Tests.Setup;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class ManagerLoggingTests
    {
        private readonly FakeLocationCacheFactory fakeLocationCacheFactory;
        private readonly CacheManager locationCache;
        private readonly LocationCreator locationCreator;

        public ManagerLoggingTests()
        {
            var serviceProvider = ConfigureDi.BuildDi();

            fakeLocationCacheFactory = serviceProvider.GetRequiredService<FakeLocationCacheFactory>();
            locationCache = fakeLocationCacheFactory.Create();
            locationCreator = Setup.ConfigureDi.Services.GetRequiredService<LocationCreator>();
        }

        [Fact]
        public async Task Lookup_logs_search_miss_in_l1()
        {
            var result = await locationCache.LookupAsync("Anything");

            Assert.Contains(fakeLocationCacheFactory.Logger.LoggedEventIds, l => l.Id == (int)LogEventIds.L1CacheManagerMiss);
        }

        [Fact]
        public async Task Lookup_logs_search_hit_in_l1()
        {
            var location = locationCreator.Create("AddedNow");
            await fakeLocationCacheFactory.Level1Cache.InsertAsync(location);

            var result = await locationCache.LookupAsync("AddedNow");

            Assert.Contains(fakeLocationCacheFactory.Logger.LoggedEventIds, l => l.Id == (int)LogEventIds.L1CacheManagerHit);
        }

        [Fact]
        public async Task Lookup_logs_search_miss_in_l2()
        {
            var result = await locationCache.LookupAsync("Anything");

            Assert.Contains(fakeLocationCacheFactory.Logger.LoggedEventIds, l => l.Id == (int)LogEventIds.L2CacheManagerMiss);
        }

        [Fact]
        public async Task Lookup_logs_search_hit_in_l2()
        {
            var location = locationCreator.Create("AddedNow");
            await fakeLocationCacheFactory.Level2Cache.InsertAsync(location);

            var result = await locationCache.LookupAsync("AddedNow");

            Assert.Contains(fakeLocationCacheFactory.Logger.LoggedEventIds, l => l.Id == (int)LogEventIds.L2CacheManagerHit);
        }
    }
}
