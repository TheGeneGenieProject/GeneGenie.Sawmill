// <copyright file="LocationCacheMemory.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Caching
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Interfaces;
    using GeneGenie.Sawmill.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// An in-process memory cache of geocoding responses used to prevent repeatedly calling
    /// an external service if we have recently looked up the location.
    /// Can be thought of as level 1 (L1) cache in a CPU architecture as it is very fast and low
    /// cost to use, but has limited data in it.
    /// </summary>
    /// <remarks>This is used for caching on a single process. If items are not found in this
    /// cache then they will go to a L2 cache service that has all of the previous results.</remarks>
    public class LocationCacheMemory : ILocationCache
    {
        private readonly ILogger<LocationCacheMemory> logger;

        public LocationCacheMemory(ILogger<LocationCacheMemory> logger)
        {
            this.logger = logger;
        }

        private Dictionary<string, SawmillGeocodeRequest> Locations { get; set; } = new Dictionary<string, SawmillGeocodeRequest>();

        public async Task InsertAsync(SawmillGeocodeRequest location)
        {
            // TODO: When .Net Standard 2.1 is released, change the following to TryAdd instead of ContainsKey and Add.
            // if (Locations.TryAdd(location.SourceKey, location))
            if (!Locations.ContainsKey(location.SourceKey))
            {
                Locations.Add(location.SourceKey, location);
                logger?.LogDebug((int)LogEventIds.L1MemoryCacheInserted, "Location '{sourceKey}' added to L1 cache", location.SourceKey);
                await Task.CompletedTask;
            }

            logger?.LogDebug((int)LogEventIds.L1MemoryCacheAlreadyExists, "Location '{sourceKey}' already exists in L1 cache", location.SourceKey);
            await Task.CompletedTask;
        }

        public async Task<SawmillGeocodeRequest> FindByKeyAsync(string sourceKey)
        {
            logger?.LogDebug((int)LogEventIds.L1MemoryCacheSearching, "Searching L1 location cache for '{sourceKey}'", sourceKey);
            if (Locations.TryGetValue(sourceKey, out var cacheItem))
            {
                logger?.LogDebug((int)LogEventIds.L1MemoryCacheFound, "Found '{sourceKey}' in L1 cache", sourceKey);
                return await Task.FromResult(cacheItem);
            }

            logger?.LogDebug((int)LogEventIds.L1MemoryCacheNotFound, "L1 '{sourceKey}' not found", sourceKey);
            return await Task.FromResult<SawmillGeocodeRequest>(null);
        }
    }
}
