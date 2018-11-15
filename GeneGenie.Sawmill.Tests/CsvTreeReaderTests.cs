// <copyright file="CsvTreeReaderTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests
{
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.IO;
    using Xunit;
    using static TestHelpers;

    public class CsvTreeReaderTests
    {
        [Fact]
        public async Task Null_file_does_not_cause_error()
        {
            var csvTreeReader = new CsvTreeReader(null, null);

            var people = await csvTreeReader.ReadAllAsync();

            Assert.Empty(people);
        }

        [Fact]
        public async Task Missing_file_does_not_cause_error()
        {
            var csvTreeReader = new CsvTreeReader("abcd", null);

            var people = await csvTreeReader.ReadAllAsync();

            Assert.Empty(people);
        }

        [Fact]
        public async Task Corrupt_file_contents_do_not_cause_error()
        {
            var csvTreeReader = new CsvTreeReader(CorruptTreeCsv, null);

            var people = await csvTreeReader.ReadAllAsync();

            Assert.Empty(people);
        }

        [Fact]
        public async Task Csv_can_be_read_and_rows_parsed()
        {
            var csvTreeReader = new CsvTreeReader(RyansTreeCsv, null);

            var people = await csvTreeReader.ReadAllAsync();

            Assert.NotEmpty(people);
        }

        [Fact]
        public async Task Expected_number_of_rows_can_be_found_after_parsing_csv()
        {
            var csvTreeReader = new CsvTreeReader(RyansTreeCsv, null);

            var people = await csvTreeReader.ReadAllAsync();

            Assert.Equal(282, people.Count);
        }
    }
}
