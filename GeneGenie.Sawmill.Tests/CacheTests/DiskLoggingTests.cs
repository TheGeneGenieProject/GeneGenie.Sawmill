// <copyright file="DiskLoggingTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests.CacheTests
{
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Caching;
    using GeneGenie.Sawmill.Models;
    using GeneGenie.Sawmill.Tests.Fakes;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class DiskLoggingTests
    {
        private readonly LocationCacheDisk locationCacheDisk;
        private readonly LocationCreator locationCreator;
        private readonly FakeLogger<LocationCacheDisk> logger;

        public DiskLoggingTests()
        {
            logger = Setup.ConfigureDi.Services.GetRequiredService<FakeLogger<LocationCacheDisk>>();
            var diskStoragePath = System.Guid.NewGuid().ToString() + ".json";
            locationCacheDisk = new LocationCacheDisk(logger, diskStoragePath);
            locationCreator = Setup.ConfigureDi.Services.GetRequiredService<LocationCreator>();
        }

        [Fact]
        public async Task Lookup_logs_search_attempt()
        {
            var result = await locationCacheDisk.FindByKeyAsync("Anything");

            Assert.Contains(logger.LoggedEventIds, l => l.Id == (int)LogEventIds.L2DiskCacheSearching);
        }

        [Fact]
        public async Task Lookup_logs_cache_miss_debug_when_not_found()
        {
            var result = await locationCacheDisk.FindByKeyAsync("Does not exist in cache");

            Assert.Contains(logger.LoggedEventIds, l => l.Id == (int)LogEventIds.L2DiskCacheNotFound);
        }

        [Fact]
        public async Task Lookup_logs_cache_hit_debug_when_found()
        {
            var location = locationCreator.Create("AddedNow");
            await locationCacheDisk.InsertAsync(location);

            var result = await locationCacheDisk.FindByKeyAsync(location.SourceKey);

            Assert.Contains(logger.LoggedEventIds, l => l.Id == (int)LogEventIds.L2DiskCacheFound);
        }

        [Fact]
        public async Task Insert_logs_cache_insert_debug_when_not_in_cache()
        {
            var location = locationCreator.Create("AddedNow");

            await locationCacheDisk.InsertAsync(location);

            Assert.Contains(logger.LoggedEventIds, l => l.Id == (int)LogEventIds.L2DiskCacheInserted);
        }

        [Fact]
        public async Task Insert_logs_cache_item_already_exists()
        {
            var location = locationCreator.Create("AddedNow");
            await locationCacheDisk.InsertAsync(location);

            await locationCacheDisk.InsertAsync(location);

            Assert.Contains(logger.LoggedEventIds, l => l.Id == (int)LogEventIds.L2DiskCacheAlreadyExists);
        }
    }
}
