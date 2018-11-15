// <copyright file="LocationCacheDisk.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Caching
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Interfaces;
    using GeneGenie.Sawmill.Models;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// A disk based cache of geocoding responses used to prevent repeatedly calling
    /// an external service if we have recently looked up the location.
    /// Can be thought of as level 2 (L2) cache in a CPU architecture as it will take longer
    /// than the L1 due to disk access.
    /// </summary>
    /// <remarks>If you want a proper scalable cache then you need to implement something like DocumentDb as a L2 cache.</remarks>
    public class LocationCacheDisk : ILocationCache
    {
        private const string DefaultStoragePath = "Locations.json";

        private readonly ILogger<LocationCacheDisk> logger;
        private readonly string storagePath;

        private Dictionary<string, SawmillGeocodeRequest> locations;

        public LocationCacheDisk(ILogger<LocationCacheDisk> logger, string storagePath = null)
        {
            this.logger = logger;
            this.storagePath = storagePath ?? DefaultStoragePath;
        }

        public async Task InsertAsync(SawmillGeocodeRequest location)
        {
            await EnsureLocationsLoaded();

            // TODO: When .Net Standard 2.1 is released, change the following to TryAdd instead of ContainsKey and Add.
            // if (Locations.TryAdd(location.SourceKey, location))
            if (!locations.ContainsKey(location.SourceKey))
            {
                locations.Add(location.SourceKey, location);
                logger?.LogDebug((int)LogEventIds.L2DiskCacheInserted, "Location '{sourceKey}' added to L2 cache", location.SourceKey);

                await SaveToDiskAsync();
                return;
            }

            logger?.LogDebug((int)LogEventIds.L2DiskCacheAlreadyExists, "Location '{sourceKey}' already exists in L2 cache", location.SourceKey);
            await Task.CompletedTask;
        }

        public async Task<SawmillGeocodeRequest> FindByKeyAsync(string sourceKey)
        {
            await EnsureLocationsLoaded();

            logger?.LogDebug((int)LogEventIds.L2DiskCacheSearching, "Searching L2 location cache for '{sourceKey}'", sourceKey);
            if (locations.TryGetValue(sourceKey, out var cacheItem))
            {
                logger?.LogDebug((int)LogEventIds.L2DiskCacheFound, "Found '{sourceKey}' in L2 cache", sourceKey);
                return await Task.FromResult(cacheItem);
            }

            logger?.LogDebug((int)LogEventIds.L2DiskCacheNotFound, "L2 '{sourceKey}' not found", sourceKey);
            return await Task.FromResult<SawmillGeocodeRequest>(null);
        }

        private async Task EnsureLocationsLoaded()
        {
            if (locations == null)
            {
                locations = await LoadFromDiskAsync();
            }
        }

        private async Task<Dictionary<string, SawmillGeocodeRequest>> LoadFromDiskAsync()
        {
            if (!File.Exists(storagePath))
            {
                return new Dictionary<string, SawmillGeocodeRequest>();
            }

            using (var sr = new StreamReader(storagePath))
            {
                var json = await sr.ReadToEndAsync();

                return JsonConvert.DeserializeObject<Dictionary<string, SawmillGeocodeRequest>>(json);
            }
        }

        private async Task SaveToDiskAsync()
        {
            var json = JsonConvert.SerializeObject(locations, Formatting.Indented);

            using (var sw = new StreamWriter(storagePath))
            {
                await sw.WriteAsync(json);
            }
        }
    }
}
