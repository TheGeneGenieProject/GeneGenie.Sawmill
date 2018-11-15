// <copyright file="JsonTreeWriterTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests
{
    using System;
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.IO;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using static TestHelpers;

    public class JsonTreeWriterTests
    {
        private readonly IServiceProvider serviceProvider;

        public JsonTreeWriterTests()
        {
            serviceProvider = Setup.ConfigureDi.BuildDi();
        }

        [Fact]
        public async Task Null_file_does_not_cause_error()
        {
            var jsonTreeWriter = new JsonTreeWriter(null, null);

            var people = await jsonTreeWriter.ReadAllAsync();

            Assert.Empty(people);
        }

        [Fact]
        public async Task Trees_can_be_written()
        {
            var csvTreeReader = new CsvTreeReader(RyansTreeCsv, null);
            var people = await csvTreeReader.ReadAllAsync();
            var treeParser = serviceProvider.GetRequiredService<TreeParser>();
            var trees = treeParser.ParseListIntoTrees(people);
            var file = new System.IO.FileInfo(Guid.NewGuid().ToString() + ".json");
            var jsonTreeWriter = new JsonTreeWriter(null, file.FullName);

            await jsonTreeWriter.WriteAllAsync(trees);

            Assert.True(file.Exists);
        }
    }
}
