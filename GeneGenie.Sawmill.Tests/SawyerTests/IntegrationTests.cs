// <copyright file="IntegrationTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using GeneGenie.DataQuality.Models;
    using GeneGenie.Sawmill.Interfaces;
    using GeneGenie.Sawmill.Models;
    using GeneGenie.Sawmill.Tests.Fakes;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    /// <summary>
    /// Tests to check that the <see cref="Sawyer"/> class interacts with storage correctly.
    /// Isolated from any real storage via in memory fake storage.
    /// </summary>
    public class IntegrationTests
    {
        private readonly FakeSawyerFactory fakeSawyerFactory;
        private readonly Sawyer sawyer;

        public IntegrationTests()
        {
            var serviceProvider = Setup.ConfigureDi.Services;
            fakeSawyerFactory = serviceProvider.GetRequiredService<FakeSawyerFactory>();
            sawyer = fakeSawyerFactory.Create();
        }

        /// <summary>
        /// Checks that <see cref="Sawyer"/> invokes whatever implementation of <see cref="ITreeReader"/> we give it.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Tree_is_loaded()
        {
            await sawyer.ProcessTreeDataAsync();

            Assert.True(fakeSawyerFactory.TreeReader.TreeHasBeenRead);
        }

        /// <summary>
        /// Checks that <see cref="Sawyer"/> invokes the implementation of <see cref="ITreeWriter"/> we pass it.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Output_is_created_when_it_does_not_exist()
        {
            await sawyer.ProcessTreeDataAsync();

            Assert.NotEmpty(fakeSawyerFactory.TreeWriter.Trees);
        }

        [Fact]
        public async Task Tree_is_not_loaded_from_source_when_destination_already_exists()
        {
            // Creates the destination trees on first pass.
            await sawyer.ProcessTreeDataAsync();
            fakeSawyerFactory.TreeReader.TreeHasBeenRead = false;

            // Loads from the destination and attempts another pass.
            await sawyer.ProcessTreeDataAsync();

            Assert.False(fakeSawyerFactory.TreeReader.TreeHasBeenRead);
        }

        [Fact]
        public async Task Output_records_marked_as_in_error_are_updated_on_second_pass()
        {
            await sawyer.ProcessTreeDataAsync();
            var person = fakeSawyerFactory.TreeWriter.Trees.First().People.First();
            person.Birth.Location.Status = SawmillStatus.TemporaryGeocodeError;

            await sawyer.ProcessTreeDataAsync();

            Assert.Equal(SawmillStatus.Geocoded, person.Birth.Location.Status);
        }

        [Fact]
        public async Task Output_birth_locations_are_updated_with_quality_check_flags()
        {
            await sawyer.ProcessTreeDataAsync();

            var person = fakeSawyerFactory.TreeWriter.Trees.First().People.Single(p => p.FirstName == "UnknownBirthPlace");
            Assert.Equal(SawmillStatus.KnownErroneous, person.Birth.Location.Status);
        }

        [Fact]
        public async Task Output_birth_dates_are_updated_with_invalid_quality_check_flags()
        {
            await sawyer.ProcessTreeDataAsync();

            var person = fakeSawyerFactory.TreeWriter.Trees.First().People.Single(p => p.FirstName == "InvalidDate");
            Assert.Equal(DateQualityStatus.NotValid, person.Birth.DateRange.Status);
        }

        [Fact]
        public async Task Output_birth_dates_are_updated_with_valid_quality_check_flags()
        {
            await sawyer.ProcessTreeDataAsync();

            var person = fakeSawyerFactory.TreeWriter.Trees.First().People.Single(p => p.FirstName == "ValidDate");
            Assert.Equal(DateQualityStatus.OK, person.Birth.DateRange.Status);
        }

        [Fact]
        public async Task Geocoder_is_called_for_locations_requiring_geocoding()
        {
            await sawyer.ProcessTreeDataAsync();

            var person = fakeSawyerFactory.TreeWriter.Trees.First().People.Single(p => p.FirstName == "ValidBirthPlace");
            Assert.Equal(SawmillStatus.Geocoded, person.Birth.Location.Status);
        }
    }
}
