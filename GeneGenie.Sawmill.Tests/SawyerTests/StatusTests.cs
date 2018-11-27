// <copyright file="StatusTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests.SawyerTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Models;
    using GeneGenie.Sawmill.Tests.Fakes;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class StatusTests
    {
        private readonly FakeSawyerFactory fakeSawyerFactory;

        public StatusTests()
        {
            var serviceProvider = Setup.ConfigureDi.Services;
            fakeSawyerFactory = serviceProvider.GetRequiredService<FakeSawyerFactory>();
        }

        [Fact]
        public async Task Ensure_all_status_enums_are_handled()
        {
            var destination = $"{Guid.NewGuid()}.json";
            var sawyer = fakeSawyerFactory.Create();
            await sawyer.ProcessTreeDataAsync();

            var peopleEvents = fakeSawyerFactory.TreeWriter.Trees;

            foreach (SawmillStatus sawmillStatus in Enum.GetValues(typeof(SawmillStatus)))
            {
                // Find the person for this enum status test.
                var person = peopleEvents.First(p => p.Who.FirstName == sawmillStatus.ToString());

                // Find the expected status of the location after geocoding.
                if (!Enum.TryParse<SawmillStatus>(person.Who.LastName, out var expectedStatus))
                {
                    throw new MissingFieldException($"There should be a row of personal data to test the enum value {sawmillStatus} but it is missing.");
                }

                Assert.Equal(expectedStatus, person.Where.Location.Status);
            }
        }
    }
}
