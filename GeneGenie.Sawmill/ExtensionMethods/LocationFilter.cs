// <copyright file="LocationFilter.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.ExtensionMethods
{
    using System.Collections.Generic;
    using System.Linq;
    using GeneGenie.DataQuality;
    using GeneGenie.Sawmill.Models;

    // TODO: Move to DQ package. Would need to map status then.
    public class LocationFilter
    {
        private static List<string> knownJunkLocations = new List<string>
        {
            "unknown",
            "?",
        };

        private readonly DateParser dateParser;

        public LocationFilter(DateParser dateParser)
        {
            this.dateParser = dateParser;
        }

        public SawmillStatus StatusGuessFromSourceQuality(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return SawmillStatus.SkippedAsEmpty;
            }

            var cleaned = source.Trim().ToLower();

            if (knownJunkLocations.Contains(cleaned))
            {
                return SawmillStatus.KnownErroneous;
            }

            if (cleaned.All(char.IsDigit))
            {
                return SawmillStatus.AllNumeric;
            }

            var date = dateParser.Parse(source);
            if (date.DateFrom != null || date.DateTo != null)
            {
                return SawmillStatus.SeemsToBeADate;
            }

            return SawmillStatus.RequiresLookup;
        }
    }
}
