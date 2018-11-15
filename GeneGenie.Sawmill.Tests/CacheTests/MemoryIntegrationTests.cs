// <copyright file="MemoryIntegrationTests.cs" company="GeneGenie.com">
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

    /// <summary>
    /// Tests to ensure the <see cref="CacheManager"/> integrates with <see cref="LocationCacheMemory"/>.
    /// </summary>
    public class MemoryIntegrationTests
    {
        private readonly KeyComposer keyComposer;
        private readonly FakeLocationCacheFactory fakeLocationCacheFactory;
        private readonly CacheManager locationCache;

        public MemoryIntegrationTests()
        {
            var serviceProvider = ConfigureDi.BuildDi();
            keyComposer = serviceProvider.GetRequiredService<KeyComposer>();
            fakeLocationCacheFactory = serviceProvider.GetRequiredService<FakeLocationCacheFactory>();
            locationCache = fakeLocationCacheFactory.Create();
        }

        [Fact]
        public async Task Location_is_not_in_l1_cache_on_first_run()
        {
            var sourceKey = keyComposer.GenerateSourceKey("Luton");

            var location = await fakeLocationCacheFactory.Level1Cache.FindByKeyAsync(sourceKey);

            Assert.Null(location);
        }

        [Fact]
        public async Task Location_is_added_to_L1_cache_after_lookup_to_prevent_repeated_lookups()
        {
            var sourceKey = keyComposer.GenerateSourceKey("Luton");
            await locationCache.LookupAsync("Luton");

            var location = await fakeLocationCacheFactory.Level1Cache.FindByKeyAsync(sourceKey);

            Assert.NotNull(location);
        }

        [Fact]
        public async Task Location_can_be_added_twice_to_L1_and_last_write_wins()
        {
            var location = fakeLocationCacheFactory.LocationCreator.Create("New place");
            location.Status = SawmillStatus.RequiresLookup;
            await fakeLocationCacheFactory.Level1Cache.InsertAsync(location);
            location.Status = SawmillStatus.Geocoded;
            await fakeLocationCacheFactory.Level1Cache.InsertAsync(location);

            location = await fakeLocationCacheFactory.Level1Cache.FindByKeyAsync(location.SourceKey);

            Assert.Equal(SawmillStatus.Geocoded, location.Status);
        }
    }
}
