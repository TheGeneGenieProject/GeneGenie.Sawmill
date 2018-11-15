// <copyright file="DiskIntegrationTests.cs" company="GeneGenie.com">
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
    /// Tests to ensure the <see cref="CacheManager"/> integrates with <see cref="LocationCacheDisk"/>.
    /// </summary>
    public class DiskIntegrationTests
    {
        private readonly KeyComposer keyComposer;
        private readonly FakeLocationCacheFactory fakeLocationCacheFactory;
        private readonly CacheManager locationCache;

        public DiskIntegrationTests()
        {
            var serviceProvider = ConfigureDi.BuildDi();
            keyComposer = serviceProvider.GetRequiredService<KeyComposer>();
            fakeLocationCacheFactory = serviceProvider.GetRequiredService<FakeLocationCacheFactory>();
            locationCache = fakeLocationCacheFactory.Create();
        }

        [Fact]
        public async Task Location_is_not_in_l2_cache_on_first_run()
        {
            var sourceKey = keyComposer.GenerateSourceKey("Luton");

            var dbEntry = await fakeLocationCacheFactory.Level2Cache.FindByKeyAsync(sourceKey);

            Assert.Null(dbEntry);
        }

        [Fact]
        public async Task Location_is_not_added_to_L2_cache_after_not_being_found()
        {
            var sourceKey = keyComposer.GenerateSourceKey("Luton");
            await locationCache.LookupAsync("Luton");

            var location = await fakeLocationCacheFactory.Level2Cache.FindByKeyAsync(sourceKey);

            Assert.Null(location);
        }

        [Fact]
        public async Task Location_is_found_in_L2_when_not_in_L1()
        {
            var location = fakeLocationCacheFactory.LocationCreator.Create("Only in L2");
            location.Status = SawmillStatus.Geocoded;
            await fakeLocationCacheFactory.Level2Cache.InsertAsync(location);

            var result = await locationCache.LookupAsync(location.Source);

            Assert.Equal(SawmillStatus.Geocoded, result.Status);
        }
    }
}
