// <copyright file="Sawyer.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GeneGenie.DataQuality;
    using GeneGenie.DataQuality.Models;
    using GeneGenie.Geocoder.Interfaces;
    using GeneGenie.Geocoder.Models.Geo;
    using GeneGenie.Sawmill.Caching;
    using GeneGenie.Sawmill.Interfaces;
    using GeneGenie.Sawmill.Models;
    using Microsoft.Extensions.Logging;

    public class Sawyer
    {
        private readonly AddressQualityChecker addressQualityChecker;
        private readonly IGeocodeManager geocodeManager;
        private readonly CacheManager locationCache;
        private readonly ILogger<Sawyer> logger;
        private readonly TreeParser treeParser;
        private readonly ITreeReader treeReader;
        private readonly ITreeWriter treeWriter;

        public Sawyer(AddressQualityChecker addressQualityChecker, IGeocodeManager geocodeManager, ILocationCacheFactory locationCacheFactory, ILogger<Sawyer> logger, TreeParser treeParser, ITreeReader treeReader, ITreeWriter treeWriter)
        {
            this.addressQualityChecker = addressQualityChecker;
            this.geocodeManager = geocodeManager;
            this.locationCache = locationCacheFactory.Create();
            this.logger = logger;
            this.treeParser = treeParser;
            this.treeReader = treeReader;
            this.treeWriter = treeWriter;
        }

        /// <summary>
        /// Takes a source and destination file and processes the person based tree data in the source
        /// so that addresses are geocoded and dates are formatted into ranges.
        /// If the destination already exists it is read to see which records do not need
        /// parameter
        /// Checks storage and geocodes every tree that has people that need geocoding.
        /// </summary>
        public async Task ProcessTreeDataAsync()
        {
            List<FamilyTree> trees;

            if (await treeWriter.TargetExistsAsync())
            {
                // If the destination already exists, load it from there to complete any records not processed before.
                trees = await treeWriter.ReadAllAsync();
            }
            else
            {
                var people = await treeReader.ReadAllAsync();
                trees = treeParser.ParseListIntoTrees(people);
            }

            // TODO: Test for null logger injected and all output below is logged to a test logger.
            logger?.LogInformation((int)LogEventIds.GeocodingLocations, "Geocoding locations.");

            foreach (var tree in trees)
            {
                logger?.LogInformation((int)LogEventIds.CheckingTree, "Checking tree {treeId} with match '{matchName}'.", tree.TreeId, tree.MatchName);
                foreach (var person in tree.People)
                {
                    person.Birth.Location = await GeocodeLocationAsync(person.Birth.Location);
                    person.Death.Location = await GeocodeLocationAsync(person.Death.Location);
                }
            }

            await treeWriter.WriteAllAsync(trees);
        }

        // TODO: I don't like that this func mutates the passed parameter, create a model and return it.
        private async Task<SawmillGeocodeRequest> GeocodeLocationAsync(SawmillGeocodeRequest location)
        {
            if (location.Status == SawmillStatus.RequiresLookup)
            {
                // TODO: Test that this caches.
                logger?.LogInformation((int)LogEventIds.CheckingCache, "Looking up locations in L1/L2 caches");
                location = await locationCache.LookupAsync(location.Source);
            }

            var qualityStatus = addressQualityChecker.StatusGuessFromSourceQuality(location.Source);
            if (qualityStatus != AddressQualityStatus.OK)
            {
                location.Status = MapQualityStatus(qualityStatus);
            }

            if (location.Status == SawmillStatus.RequiresGeocoding || location.Status == SawmillStatus.TemporaryGeocodeError)
            {
                // TODO: We might need to record the last time a geocoding was attempted and how many tries if this was a temp error so we don't keep retrying.
                var result = await geocodeManager.GeocodeAddressAsync(location.Source);

                // TODO: Insert additional responses into cache here plus expiry times dependent on the status.
                if (result.Status == AddressLookupStatus.Geocoded || result.Status == AddressLookupStatus.ZeroResults)
                {
                    // TODO: Add cache inserting.
                    // locationCache.InsertAsync
                }

                // TODO: Geocoder should return a text description of the errors so we can present to the user.
                location.Status = MapGeocoderStatus(result.Status);
                location.GeocoderUsed = result.GeocoderId;
                if (location.Status == SawmillStatus.Geocoded)
                {
                    location.Responses = result.Locations;
                }
            }

            return location;
        }

        private SawmillStatus MapQualityStatus(AddressQualityStatus status)
        {
            if (status == AddressQualityStatus.AllNumeric)
            {
                return SawmillStatus.AllNumeric;
            }

            if (status == AddressQualityStatus.KnownErroneous)
            {
                return SawmillStatus.KnownErroneous;
            }

            if (status == AddressQualityStatus.SeemsToBeADate)
            {
                return SawmillStatus.SeemsToBeADate;
            }

            return SawmillStatus.SkippedAsEmpty;
        }

        private SawmillStatus MapGeocoderStatus(AddressLookupStatus status)
        {
            if (status == AddressLookupStatus.Geocoded)
            {
                return SawmillStatus.Geocoded;
            }

            if (status == AddressLookupStatus.ZeroResults)
            {
                return SawmillStatus.ZeroResults;
            }

            if (status == AddressLookupStatus.TemporaryGeocodeError)
            {
                return SawmillStatus.TemporaryGeocodeError;
            }

            return SawmillStatus.RequiresUserIntervention;
        }
    }
}
