// <copyright file="SawmillGeocodeRequest.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Models
{
    using System.Collections.Generic;
    using GeneGenie.Geocoder.Models.Geo;
    using GeneGenie.Geocoder.Services;

    // TODO: Too much in here and named wrong.
    public class SawmillGeocodeRequest
    {
        public string ErrorMessage { get; set; }

        public GeocoderNames GeocoderUsed { get; set; }

        public List<GeocodeResponseLocation> Responses { get; set; }

        public string Source { get; set; }

        public string SourceKey { get; set; }

        public SawmillStatus Status { get; set; }
    }
}
