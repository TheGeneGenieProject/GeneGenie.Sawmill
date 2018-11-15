// <copyright file="TestHelpers.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.IO;
    using GeneGenie.Sawmill.Models;
    using GeneGenie.Sawmill.Tests.Setup;
    using Microsoft.Extensions.DependencyInjection;

    public static class TestHelpers
    {
        public const string CorruptTreeCsv = @"Data\Corrupt tree.csv";
        public const string RyansTreeCsv = @"Data\Ryan's family tree.csv";
        public const string TwoTreesCsv = @"Data\Two trees.csv";

        public static async Task<List<FamilyTree>> ParseTreesFromFilePathAsync(string filePath)
        {
            var csvLoader = new CsvTreeReader(filePath, null);
            var imported = await csvLoader.ReadAllAsync();
            var treeParser = ConfigureDi.Services.GetRequiredService<TreeParser>();

            return treeParser.ParseListIntoTrees(imported);
        }
    }
}
