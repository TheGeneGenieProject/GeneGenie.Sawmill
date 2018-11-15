// <copyright file="DiskUnitTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests.CacheTests
{
    using System;
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Caching;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class DiskUnitTests
    {
        [Fact]
        public async Task Location_cache_can_be_used_when_cache_file_has_not_been_written_yet()
        {
            var locationCacheDisk = new LocationCacheDisk(null, Guid.NewGuid().ToString());

            var result = await locationCacheDisk.FindByKeyAsync("Does not exist in cache");

            Assert.Null(result);
        }

        [Fact]
        public async Task Location_cache_is_written_to_disk()
        {
            var storagePath = Guid.NewGuid().ToString();
            var locationCacheDisk = new LocationCacheDisk(null, storagePath);
            var locationCreator = Setup.ConfigureDi.Services.GetRequiredService<LocationCreator>();
            var location = locationCreator.Create("AddedToCacheHere");

            await locationCacheDisk.InsertAsync(location);

            Assert.True(System.IO.File.Exists(storagePath));
        }

        [Fact]
        public async Task Location_cache_is_read_from_disk()
        {
            var storagePath = Guid.NewGuid().ToString();
            var locationCacheDisk = new LocationCacheDisk(null, storagePath);
            var locationCreator = Setup.ConfigureDi.Services.GetRequiredService<LocationCreator>();
            var location = locationCreator.Create("PersistedToDisk");
            await locationCacheDisk.InsertAsync(location);
            locationCacheDisk = new LocationCacheDisk(null, storagePath);

            var result = await locationCacheDisk.FindByKeyAsync(location.SourceKey);

            Assert.Equal("PersistedToDisk", result.Source);
        }

        [Fact]
        public void Disk_cache_does_not_exist_when_initialised_but_not_yet_used()
        {
            var storagePath = Guid.NewGuid().ToString();
            var locationCacheDisk = new LocationCacheDisk(null, storagePath);

            Assert.False(System.IO.File.Exists(storagePath));
        }
    }
}
