// <copyright file="LocationTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests.TreeAnalysis
{
    using System.Linq;
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.IO;
    using Xunit;

    public class LocationTests
    {
        private readonly TreeAnalyser treeAnalyser;

        public LocationTests()
        {
            treeAnalyser = new TreeAnalyser();
        }

        [Fact]
        public async Task Invalid_locations_raise_flag()
        {
            var jsonTreeWriter = new JsonTreeWriter(null, @"Data\\Processed tree.json");
            var trees = await jsonTreeWriter.ReadAllAsync();

            var treeStatistics = treeAnalyser.GenerateStatistics(trees);

            Assert.True(treeStatistics.LocationIssuesExist);
        }

        [Fact]
        public async Task Location_breakdown_is_correct()
        {
            var jsonTreeWriter = new JsonTreeWriter(null, @"Data\\Processed tree.json");
            var trees = await jsonTreeWriter.ReadAllAsync();

            var treeStatistics = treeAnalyser.GenerateStatistics(trees);

            Assert.Equal(0, treeStatistics.LocationCountsByStatus.Single(d => d.Status == Models.SawmillStatus.AllNumeric).Count);
            Assert.Equal(330, treeStatistics.LocationCountsByStatus.Single(d => d.Status == Models.SawmillStatus.Geocoded).Count);
            Assert.Equal(134, treeStatistics.LocationCountsByStatus.Single(d => d.Status == Models.SawmillStatus.KnownErroneous).Count);
            Assert.Equal(0, treeStatistics.LocationCountsByStatus.Single(d => d.Status == Models.SawmillStatus.RequiresGeocoding).Count);
            Assert.Equal(0, treeStatistics.LocationCountsByStatus.Single(d => d.Status == Models.SawmillStatus.RequiresLookup).Count);
            Assert.Equal(0, treeStatistics.LocationCountsByStatus.Single(d => d.Status == Models.SawmillStatus.RequiresUserIntervention).Count);
            Assert.Equal(0, treeStatistics.LocationCountsByStatus.Single(d => d.Status == Models.SawmillStatus.SeemsToBeADate).Count);
            Assert.Equal(100, treeStatistics.LocationCountsByStatus.Single(d => d.Status == Models.SawmillStatus.SkippedAsEmpty).Count);
            Assert.Equal(0, treeStatistics.LocationCountsByStatus.Single(d => d.Status == Models.SawmillStatus.TemporaryGeocodeError).Count);
            Assert.Equal(0, treeStatistics.LocationCountsByStatus.Single(d => d.Status == Models.SawmillStatus.ZeroResults).Count);
        }
    }
}
