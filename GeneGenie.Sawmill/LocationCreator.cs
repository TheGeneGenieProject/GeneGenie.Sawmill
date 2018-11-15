// <copyright file="LocationCreator.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill
{
    using GeneGenie.Sawmill.ExtensionMethods;
    using GeneGenie.Sawmill.Models;

    public class LocationCreator
    {
        private readonly KeyComposer keyComposer;
        private readonly LocationFilter locationFilter;

        public LocationCreator(KeyComposer keyComposer, LocationFilter locationFilter)
        {
            this.keyComposer = keyComposer;
            this.locationFilter = locationFilter;
        }

        public SawmillGeocodeRequest Create(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                source = string.Empty;
            }

            return new SawmillGeocodeRequest
            {
                Source = source,
                SourceKey = keyComposer.GenerateSourceKey(source),
                Status = locationFilter.StatusGuessFromSourceQuality(source),
            };
        }
    }
}
