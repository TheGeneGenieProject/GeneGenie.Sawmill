// <copyright file="LocationFilterTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests
{
    using System.Collections.Generic;
    using GeneGenie.Sawmill.ExtensionMethods;
    using GeneGenie.Sawmill.Models;
    using GeneGenie.Sawmill.Tests.Setup;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class LocationFilterTests
    {
        private readonly LocationFilter locationFilter;

        public LocationFilterTests()
        {
            var services = ConfigureDi.BuildDi();
            locationFilter = services.GetRequiredService<LocationFilter>();
        }

        public static IEnumerable<object[]> WhitespaceKeyValueData()
        {
            yield return new object[] { null, SawmillStatus.SkippedAsEmpty };
            yield return new object[] { string.Empty, SawmillStatus.SkippedAsEmpty };
            yield return new object[] { " ", SawmillStatus.SkippedAsEmpty };
            yield return new object[] { "  ", SawmillStatus.SkippedAsEmpty };
            yield return new object[] { " \t ", SawmillStatus.SkippedAsEmpty };
            yield return new object[] { " \n\r \r\n \t ", SawmillStatus.SkippedAsEmpty };
        }

        public static IEnumerable<object[]> DateKeyValueData()
        {
            yield return new object[] { "Jan 1 2000", SawmillStatus.SeemsToBeADate };
            yield return new object[] { "1 Jan 2000", SawmillStatus.SeemsToBeADate };
            yield return new object[] { "1/1/2000", SawmillStatus.SeemsToBeADate };
            yield return new object[] { "1\\1\\2000", SawmillStatus.SeemsToBeADate };
        }

        public static IEnumerable<object[]> OtherJunkKeyValueData()
        {
            yield return new object[] { "1", SawmillStatus.AllNumeric };
            yield return new object[] { "UnKnown", SawmillStatus.KnownErroneous };
        }

        [Theory]
        [MemberData(nameof(WhitespaceKeyValueData))]
        public void Locations_are_detected_as_whitespace(string source, SawmillStatus expected)
        {
            var status = locationFilter.StatusGuessFromSourceQuality(source);

            Assert.Equal(expected, status);
        }

        [Theory]
        [MemberData(nameof(DateKeyValueData))]
        public void Locations_are_detected_as_dates(string source, SawmillStatus expected)
        {
            var status = locationFilter.StatusGuessFromSourceQuality(source);

            Assert.Equal(expected, status);
        }

        [Theory]
        [MemberData(nameof(OtherJunkKeyValueData))]
        public void Locations_are_filtered_of_known_junk_formats(string source, SawmillStatus expected)
        {
            var status = locationFilter.StatusGuessFromSourceQuality(source);

            Assert.Equal(expected, status);
        }
    }
}
