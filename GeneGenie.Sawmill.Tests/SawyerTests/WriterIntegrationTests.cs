// <copyright file="WriterIntegrationTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Models;
    using GeneGenie.Sawmill.Tests.Fakes;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using static TestHelpers;

    /// <summary>
    /// Tests to check that the CSV storage can be read and written to through the <see cref="Sawyer"/> class.
    /// </summary>
    public class WriterIntegrationTests
    {
        private readonly FakeSawyerFactory fakeSawyerFactory;

        public WriterIntegrationTests()
        {
            fakeSawyerFactory = Setup.ConfigureDi.Services.GetRequiredService<FakeSawyerFactory>();
        }

        [Fact]
        public void Sawyer_can_be_instantiated()
        {
            var sawyer = fakeSawyerFactory.CreateWithLocalIO(string.Empty, string.Empty);

            Assert.NotNull(sawyer);
        }

        [Fact]
        public async Task Tree_can_be_loaded()
        {
            var destination = $"{Guid.NewGuid()}.json";
            var sawyer = fakeSawyerFactory.CreateWithLocalIO(RyansTreeCsv, destination);

            await sawyer.ProcessTreeDataAsync();
        }

        /// <summary>
        /// Checks that when given new data, <see cref="Sawyer"/> will output a new dataset.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Output_is_created_when_it_does_not_exist()
        {
            var destination = new FileInfo($"{Guid.NewGuid()}.json");
            var sawyer = fakeSawyerFactory.CreateWithLocalIO(RyansTreeCsv, destination.FullName);

            await sawyer.ProcessTreeDataAsync();

            Assert.True(destination.Exists);
        }

        [Fact]
        public async Task Tree_can_be_processed_to_new_file_and_new_file_can_be_loaded_by_the_writer()
        {
            var destination = new FileInfo($"{Guid.NewGuid()}.json");
            var sawyer = fakeSawyerFactory.CreateWithLocalIO(RyansTreeCsv, destination.FullName);
            await sawyer.ProcessTreeDataAsync();

            // Should reload the tree from the destination as it already exists.
            await sawyer.ProcessTreeDataAsync();

            Assert.Equal(0, fakeSawyerFactory.WriterLogger.CriticalCount);
            Assert.Equal(0, fakeSawyerFactory.WriterLogger.ErrorCount);
            Assert.Equal(0, fakeSawyerFactory.WriterLogger.WarningCount);
        }
    }
}
