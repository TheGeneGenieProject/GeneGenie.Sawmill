// <copyright file="EmptyTreeTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests.TreeAnalysis
{
    using System.Linq;
    using System.Threading.Tasks;
    using GeneGenie.DataQuality.Models;
    using GeneGenie.Sawmill.IO;
    using Xunit;

    public class EmptyTreeTests
    {
        private readonly TreeAnalyser treeAnalyser;

        public EmptyTreeTests()
        {
            treeAnalyser = new TreeAnalyser();
        }

        [Fact]
        public async Task Empty_tree_results_in_non_null_stats()
        {
            var jsonTreeWriter = new JsonTreeWriter(null, string.Empty);
            var trees = await jsonTreeWriter.ReadAllAsync();

            var treeStatistics = treeAnalyser.GenerateStatistics(trees);

            Assert.NotNull(treeStatistics);
        }

        [Fact]
        public void Null_tree_results_in_non_null_stats()
        {
            var treeStatistics = treeAnalyser.GenerateStatistics(null);

            Assert.NotNull(treeStatistics);
        }

        [Fact]
        public async Task Error_is_indicated_when_file_does_not_exist()
        {
            var jsonTreeWriter = new JsonTreeWriter(null, $@"Data\\{System.Guid.NewGuid()}.json");
            var trees = await jsonTreeWriter.ReadAllAsync();

            var treeStatistics = treeAnalyser.GenerateStatistics(trees);

            Assert.True(treeStatistics.TreesAreEmpty);
        }

        [Fact]
        public async Task Error_is_indicated_when_file_is_empty()
        {
            var jsonTreeWriter = new JsonTreeWriter(null, $@"Data\\Empty.json");
            var trees = await jsonTreeWriter.ReadAllAsync();

            var treeStatistics = treeAnalyser.GenerateStatistics(trees);

            Assert.True(treeStatistics.TreesAreEmpty);
        }

        [Fact]
        public async Task Error_is_indicated_people_are_not_found()
        {
            var jsonTreeWriter = new JsonTreeWriter(null, $@"Data\\Empty.json");
            var trees = await jsonTreeWriter.ReadAllAsync();

            var treeStatistics = treeAnalyser.GenerateStatistics(trees);

            Assert.True(treeStatistics.PeopleNotFound);
        }
    }
}
