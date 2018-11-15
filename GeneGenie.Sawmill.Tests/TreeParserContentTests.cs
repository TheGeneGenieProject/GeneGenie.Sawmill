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
    using GeneGenie.Sawmill.ExtensionMethods;
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
            var trees = await ParseTreesFromFilePathAsync(RyansTreeCsv);

            var person = trees[0].People.Single(p => p.FirstName == "Roger");

            Assert.Equal(DateRangeScope.ExactDateWithTimeRange, person.Birth.DateRange.Scope);
        }

        [Fact]
        public async Task Birth_date_is_accurate_and_is_spread_over_the_whole_day()
        {
            var trees = await ParseTreesFromFilePathAsync(RyansTreeCsv);

            var person = trees[0].People.Single(p => p.FirstName == "Roger");

            Assert.Equal(new DateTime(1940, 7, 25), person.Birth.DateRange.DateFrom);
            Assert.Equal(new DateTime(1940, 7, 25).EndOfDay(), person.Birth.DateRange.DateTo);
        }

        [Fact]
        public async Task Person_can_be_found_by_using_three_part_name()
        {
            var trees = await ParseTreesFromFilePathAsync(RyansTreeCsv);

            var person = trees[0].People
                .Where(p => p.FirstName == "Roger")
                .Where(p => p.MiddleName == "Francis")
                .Where(p => p.LastName == "O'Neill");

            Assert.Single(person);
        }
    }
}
