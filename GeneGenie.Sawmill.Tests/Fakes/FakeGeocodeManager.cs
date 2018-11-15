// <copyright file="FakeGeocodeManager.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests.Fakes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GeneGenie.Geocoder.Interfaces;
    using GeneGenie.Geocoder.Models.Geo;
    using GeneGenie.Geocoder.Services;
    using GeneGenie.Sawmill.Models;

    public class FakeGeocodeManager : IGeocodeManager
    {
        private static readonly Dictionary<string, GeocodeResponse> FakeResults = new Dictionary<string, GeocodeResponse>
        {
            { "Bedfordshire", CreateFakeResponse(GeocoderNames.Bing, AddressLookupStatus.Geocoded) },
            { "10 Downing Street, London", CreateFakeResponse(GeocoderNames.Bing, AddressLookupStatus.Geocoded) },
            { nameof(SawmillStatus.Geocoded), CreateFakeResponse(GeocoderNames.Bing, AddressLookupStatus.Geocoded) },
            { nameof(SawmillStatus.RequiresLookup), CreateFakeResponse(GeocoderNames.Bing, AddressLookupStatus.Geocoded) },
            { nameof(SawmillStatus.RequiresGeocoding), CreateFakeResponse(GeocoderNames.Bing, AddressLookupStatus.Geocoded) },
            { nameof(SawmillStatus.TemporaryGeocodeError), CreateFakeResponse(GeocoderNames.Bing, AddressLookupStatus.TemporaryGeocodeError) },
            { nameof(SawmillStatus.RequiresUserIntervention), CreateFakeResponse(GeocoderNames.Bing, AddressLookupStatus.MultipleIssues) },
        };

        public async Task<GeocodeResponse> GeocodeAddressAsync(string address)
        {
            if (FakeResults.TryGetValue(address, out var response))
            {
                return await Task.FromResult(response);
            }

            var zeroResults = new GeocodeResponse
            {
                GeocoderId = GeocoderNames.Bing,
                Locations = new List<GeocodeResponseLocation> { },
                Status = AddressLookupStatus.ZeroResults,
            };

            return await Task.FromResult(zeroResults);
        }

        private static GeocodeResponse CreateFakeResponse(GeocoderNames geocoderName, AddressLookupStatus addressLookupStatus)
        {
            return new GeocodeResponse
            {
                GeocoderId = geocoderName,
                Locations = new List<GeocodeResponseLocation>
                {
                    new GeocodeResponseLocation
                    {
                        Bounds = new Bounds
                        {
                            NorthEast = new LocationPair { Latitude = 0, Longitude = 1 },
                            SouthWest = new LocationPair { Latitude = 2, Longitude = 3 },
                        },
                    },
                },
                Status = addressLookupStatus,
            };
        }
    }
}
