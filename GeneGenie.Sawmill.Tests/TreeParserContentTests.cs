// <copyright file="TreeParserContentTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using GeneGenie.DataQuality.ExtensionMethods;
    using GeneGenie.DataQuality.Models;
    using GeneGenie.Sawmill.Tests.Setup;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using static TestHelpers;

    public class TreeParserContentTests
    {
        private readonly TreeParser treeParser;

        public TreeParserContentTests()
        {
            treeParser = ConfigureDi.Services.GetRequiredService<TreeParser>();
        }

        [Fact]
        public async Task Birth_date_is_parsed_as_single_day_scope()
        {
            var personEvents = await ParseTreesFromFilePathAsync(RyansTreeCsv);

            var person = personEvents
                .Single(p => p.Who.FirstName == "Roger" && p.What.EventType == Models.PersonEventType.Birth);

            Assert.Equal(DateRangeScope.ExactDateWithTimeRange, person.When.DateRange.Scope);
        }

        [Fact]
        public async Task Birth_date_is_accurate_and_is_spread_over_the_whole_day()
        {
            var personEvents = await ParseTreesFromFilePathAsync(RyansTreeCsv);

            var person = personEvents
                .Single(p => p.Who.FirstName == "Roger" && p.What.EventType == Models.PersonEventType.Birth);

            Assert.Equal(new DateTime(1940, 7, 25), person.When.DateRange.DateFrom);
            Assert.Equal(new DateTime(1940, 7, 25).EndOfDay(), person.When.DateRange.DateTo);
        }

        [Fact]
        public async Task Multiple_events_can_be_found_by_using_three_part_name()
        {
            var allPersonEvents = await ParseTreesFromFilePathAsync(RyansTreeCsv);

            var personEvents = allPersonEvents
                .Where(p => p.Who.FirstName == "Roger")
                .Where(p => p.Who.MiddleName == "Francis")
                .Where(p => p.Who.LastName == "O'Neill");

            Assert.Equal(2, personEvents.Count());
        }
    }
}
