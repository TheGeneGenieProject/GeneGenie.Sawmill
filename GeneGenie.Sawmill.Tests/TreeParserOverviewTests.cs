// <copyright file="TreeParserOverviewTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests
{
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Tests.Setup;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using static TestHelpers;

    public class TreeParserOverviewTests
    {
        private readonly TreeParser treeParser;

        public TreeParserOverviewTests()
        {
            treeParser = ConfigureDi.Services.GetRequiredService<TreeParser>();
        }

        [Fact]
        public void Trees_are_empty_after_import_of_null_records()
        {
            var trees = treeParser.ParseListIntoTrees(null);

            Assert.Empty(trees);
        }

        [Fact]
        public void Trees_are_empty_after_import_of_empty_records()
        {
            var trees = treeParser.ParseListIntoTrees(new System.Collections.Generic.List<Models.PersonImport>());

            Assert.Empty(trees);
        }

        [Fact]
        public async Task Trees_are_not_empty_after_import()
        {
            var trees = await ParseTreesFromFilePathAsync(@"Data\Ryan's family tree.csv");

            Assert.NotEmpty(trees);
        }

        [Fact]
        public async Task Single_tree_is_found_after_import()
        {
            var trees = await ParseTreesFromFilePathAsync(@"Data\Ryan's family tree.csv");

            Assert.Single(trees);
        }

        [Fact]
        public async Task Two_trees_are_found_after_import()
        {
            var trees = await ParseTreesFromFilePathAsync(@"Data\Two trees.csv");

            Assert.Equal(2, trees.Count);
        }
    }
}
