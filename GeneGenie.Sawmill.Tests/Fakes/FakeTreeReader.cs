// <copyright file="FakeTreeReader.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests.Fakes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Interfaces;
    using GeneGenie.Sawmill.Models;

    public class FakeTreeReader : ITreeReader
    {
        private readonly List<PersonImport> people = new List<PersonImport>
        {
            new PersonImport { BirthDate = "20 10 1970", BirthPlace = "Bedfordshire", ContactId = "1" },
            new PersonImport { FirstName = "UnknownBirthPlace", ContactId = "2", BirthPlace = "unknown" },
            new PersonImport { FirstName = "InvalidDate", ContactId = "3", BirthDate = "unknown" },
            new PersonImport { FirstName = "ValidDate", ContactId = "4", BirthDate = "Jan 1 1990" },
            new PersonImport { FirstName = "ValidBirthPlace", ContactId = "5", BirthPlace = "10 Downing Street, London" },

            // The following is for testing that every status enum is processed.
            // BirthPlace holds the text to be geocoded, FirstName holds the enum status we are checking and LastName holds what it should be after processing.
            new PersonImport { BirthPlace = nameof(SawmillStatus.Geocoded), FirstName = nameof(SawmillStatus.Geocoded), LastName = nameof(SawmillStatus.Geocoded) },
            new PersonImport { BirthPlace = nameof(SawmillStatus.RequiresUserIntervention), FirstName = nameof(SawmillStatus.RequiresUserIntervention), LastName = nameof(SawmillStatus.RequiresUserIntervention) },
            new PersonImport { BirthPlace = nameof(SawmillStatus.TemporaryGeocodeError), FirstName = nameof(SawmillStatus.TemporaryGeocodeError), LastName = nameof(SawmillStatus.TemporaryGeocodeError) },
            new PersonImport { BirthPlace = nameof(SawmillStatus.ZeroResults), FirstName = nameof(SawmillStatus.ZeroResults), LastName = nameof(SawmillStatus.ZeroResults) },

            new PersonImport { BirthPlace = "unknown", FirstName = nameof(SawmillStatus.KnownErroneous), LastName = nameof(SawmillStatus.KnownErroneous) },
            new PersonImport { BirthPlace = nameof(SawmillStatus.RequiresGeocoding), FirstName = nameof(SawmillStatus.RequiresGeocoding), LastName = nameof(SawmillStatus.Geocoded) },
            new PersonImport { BirthPlace = nameof(SawmillStatus.RequiresLookup), FirstName = nameof(SawmillStatus.RequiresLookup), LastName = nameof(SawmillStatus.Geocoded) },
            new PersonImport { BirthPlace = "Jan 1 1900", FirstName = nameof(SawmillStatus.SeemsToBeADate), LastName = nameof(SawmillStatus.SeemsToBeADate) },
            new PersonImport { BirthPlace = string.Empty, FirstName = nameof(SawmillStatus.SkippedAsEmpty), LastName = nameof(SawmillStatus.SkippedAsEmpty) },
            new PersonImport { BirthPlace = "1234", FirstName = nameof(SawmillStatus.AllNumeric), LastName = nameof(SawmillStatus.AllNumeric) },
        };

        public bool TreeHasBeenRead { get; set; }

        public async Task<List<PersonImport>> ReadAllAsync()
        {
            TreeHasBeenRead = true;
            return await Task.FromResult(people);
        }
    }
}
