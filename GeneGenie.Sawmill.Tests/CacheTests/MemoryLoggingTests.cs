// <copyright file="MemoryLoggingTests.cs" company="GeneGenie.com">
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

    public class MemoryLoggingTests
    {
        private readonly LocationCacheMemory locationCacheMemory;
        private readonly LocationCreator locationCreator;
        private readonly FakeLogger<LocationCacheMemory> logger;

        public MemoryLoggingTests()
        {
            logger = Setup.ConfigureDi.Services.GetRequiredService<FakeLogger<LocationCacheMemory>>();
            var diskStoragePath = System.Guid.NewGuid().ToString() + ".json";
            locationCacheMemory = new LocationCacheMemory(logger);
            locationCreator = Setup.ConfigureDi.Services.GetRequiredService<LocationCreator>();
        }

        [Fact]
        public async Task Lookup_logs_search_attempt()
        {
            var result = await locationCacheMemory.FindByKeyAsync("Anything");

            Assert.Contains(logger.LoggedEventIds, l => l.Id == (int)LogEventIds.L1MemoryCacheSearching);
        }

        [Fact]
        public async Task Lookup_logs_cache_miss_debug_when_not_found()
        {
            var result = await locationCacheMemory.FindByKeyAsync("Does not exist in cache");

            Assert.Contains(logger.LoggedEventIds, l => l.Id == (int)LogEventIds.L1MemoryCacheNotFound);
        }

        [Fact]
        public async Task Lookup_logs_cache_hit_debug_when_found()
        {
            var location = locationCreator.Create("AddedNow");
            await locationCacheMemory.InsertAsync(location);

            var result = await locationCacheMemory.FindByKeyAsync(location.SourceKey);

            Assert.Contains(logger.LoggedEventIds, l => l.Id == (int)LogEventIds.L1MemoryCacheFound);
        }

        [Fact]
        public async Task Insert_logs_cache_insert_debug_when_not_in_cache()
        {
            var location = locationCreator.Create("AddedNow");

            await locationCacheMemory.InsertAsync(location);

            Assert.Contains(logger.LoggedEventIds, l => l.Id == (int)LogEventIds.L1MemoryCacheInserted);
        }

        [Fact]
        public async Task Insert_logs_cache_item_already_exists()
        {
            var location = locationCreator.Create("AddedNow");
            await locationCacheMemory.InsertAsync(location);

            await locationCacheMemory.InsertAsync(location);

            Assert.Contains(logger.LoggedEventIds, l => l.Id == (int)LogEventIds.L1MemoryCacheAlreadyExists);
        }
    }
}
