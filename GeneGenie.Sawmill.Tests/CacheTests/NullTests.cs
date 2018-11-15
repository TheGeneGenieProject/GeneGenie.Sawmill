// <copyright file="NullTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests.CacheTests
{
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Caching;
    using GeneGenie.Sawmill.Tests.Setup;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class NullTests
    {
        private readonly CacheManager locationCache;

        public NullTests()
        {
            var serviceProvider = ConfigureDi.BuildDi();
            var locationCacheFactory = serviceProvider.GetRequiredService<LocationCacheFactory>();
            locationCache = locationCacheFactory.Create();
        }

        [Fact]
        public async Task Null_returns_empty_location_without_error()
        {
            var location = await locationCache.LookupAsync(null as string);

            Assert.NotNull(location);
        }

        [Fact]
        public async Task Empty_returns_empty_location_without_error()
        {
            var location = await locationCache.LookupAsync(string.Empty);

            Assert.NotNull(location);
        }

        [Fact]
        public async Task Whitespace_returns_empty_location_without_error()
        {
            var location = await locationCache.LookupAsync("   ");

            Assert.NotNull(location);
        }

        [Fact]
        public async Task Different_whitespace_generates_same_sourcekey()
        {
            var location1 = await locationCache.LookupAsync("   ");
            var location2 = await locationCache.LookupAsync(" ");

            Assert.Equal(location1.SourceKey, location2.SourceKey);
        }
    }
}
