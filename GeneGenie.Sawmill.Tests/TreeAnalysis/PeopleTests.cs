// <copyright file="PeopleTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests.TreeAnalysis
{
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.IO;
    using Xunit;

    public class PeopleTests
    {
        private readonly TreeAnalyser treeAnalyser;

        public PeopleTests()
        {
            treeAnalyser = new TreeAnalyser();
        }

        [Fact]
        public async Task Statistics_can_be_generated_for_valid_file()
        {
            var jsonTreeWriter = new JsonTreeWriter(null, @"Data\\Processed tree.json");
            var trees = await jsonTreeWriter.ReadAllAsync();

            var treeStatistics = treeAnalyser.GenerateStatistics(trees);

            Assert.NotNull(treeStatistics);
        }

        [Fact]
        public async Task Count_of_trees_within_file_is_correct()
        {
            var jsonTreeWriter = new JsonTreeWriter(null, @"Data\\Processed tree.json");
            var trees = await jsonTreeWriter.ReadAllAsync();

            var treeStatistics = treeAnalyser.GenerateStatistics(trees);

            Assert.Equal(1, treeStatistics.NumberOfTrees);
        }

        [Fact]
        public async Task Count_of_people_within_file_is_correct()
        {
            var jsonTreeWriter = new JsonTreeWriter(null, @"Data\\Processed tree.json");
            var trees = await jsonTreeWriter.ReadAllAsync();

            var treeStatistics = treeAnalyser.GenerateStatistics(trees);

            Assert.Equal(282, treeStatistics.NumberOfPeople);
        }
    }
}
