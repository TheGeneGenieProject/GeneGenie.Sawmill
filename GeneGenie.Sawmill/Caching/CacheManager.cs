// <copyright file="CacheManager.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Caching
{
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Interfaces;
    using GeneGenie.Sawmill.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Service to enable access to the 1st and 2nd level geolocation caches before the expensive
    /// geolocation service is used.
    /// </summary>
    public class CacheManager
    {
        private readonly ILocationCache level1Cache;
        private readonly ILocationCache level2Cache;
        private readonly LocationCreator locationCreator;
        private readonly ILogger<CacheManager> logger;

        public CacheManager(ILocationCache level1Cache, ILocationCache level2Cache, LocationCreator locationCreator, ILogger<CacheManager> logger)
        {
            this.level1Cache = level1Cache;
            this.level2Cache = level2Cache;
            this.locationCreator = locationCreator;
            this.logger = logger;
        }

        public async Task<SawmillGeocodeRequest> LookupAsync(string value)
        {
            var location = locationCreator.Create(value);

            if (location.Status != SawmillStatus.RequiresLookup)
            {
                return location; // Failed to validate as worth looking up.
            }

            // Is this value already in our L1 local (memory) cache?
            var cachedLocation = await level1Cache.FindByKeyAsync(location.SourceKey);
            if (cachedLocation != null)
            {
                logger?.LogDebug((int)LogEventIds.L1CacheManagerHit, "L1 hit: '{value}'", value);
                return cachedLocation;
            }

            logger?.LogInformation((int)LogEventIds.L1CacheManagerMiss, "Cache miss: '{value}'", value);

            // No, is it in our L2 cheap storage db?
            cachedLocation = await level2Cache.FindByKeyAsync(location.SourceKey);
            if (cachedLocation != null)
            {
                logger?.LogDebug((int)LogEventIds.L2CacheManagerHit, "L2 hit: '{value}'", value);
                return cachedLocation;
            }

            logger?.LogInformation((int)LogEventIds.L2CacheManagerMiss, "Cache miss: '{value}'", value);
            location.Status = SawmillStatus.RequiresGeocoding;

            // Cache this location in L1 (local) so we don't keep checking L2 (remote) if the same key comes up again.
            await level1Cache.InsertAsync(location);

            return location;
        }
    }
}
