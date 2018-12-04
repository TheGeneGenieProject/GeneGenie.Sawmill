// <copyright file="TreeParser.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill
{
    using System.Collections.Generic;
    using System.Linq;
    using GeneGenie.DataQuality;
    using GeneGenie.Sawmill.ExtensionMethods;
    using GeneGenie.Sawmill.Models;

    public class TreeParser
    {
        private readonly DateParser dateParser;
        private readonly LocationCreator locationCreator;

        public TreeParser(DateParser dateParser, LocationCreator locationCreator)
        {
            this.dateParser = dateParser;
            this.locationCreator = locationCreator;
        }

        public List<WhoWhatWhereWhen> ParseListIntoTrees(List<PersonImport> imported)
        {
            if (imported == null)
            {
                return new List<WhoWhatWhereWhen>();
            }

            var birthEvents = imported
                .Select(p => new WhoWhatWhereWhen
                {
                    What = new What { EventType = PersonEventType.Birth },
                    When = new When { DateRange = dateParser.Parse(p.BirthDate) },
                    Where = new Where { Location = locationCreator.Create(p.BirthPlace) },
                    Who = ProjectPersonToWho(p),
                })
                .ToList();
            var deathEvents = imported
                .Select(p => new WhoWhatWhereWhen
                {
                    What = new What { EventType = PersonEventType.Death },
                    When = new When { DateRange = dateParser.Parse(p.DeathDate) },
                    Where = new Where { Location = locationCreator.Create(p.DeathPlace) },
                    Who = ProjectPersonToWho(p),
                })
                .ToList();
            return birthEvents.Concat(deathEvents).ToList();
        }

        private Who ProjectPersonToWho(PersonImport person)
        {
            return new Who
            {
                FatherId = person.FatherId.NullSafeTrim(),
                FirstName = person.FirstName.NullSafeTrim(),
                Gender = ParseGender(person.Gender),
                Generation = ParseGeneration(person.Generation),
                Id = person.TreeId.NullSafeTrim(),
                LastName = person.LastName.NullSafeTrim(),
                MatchName = person.MatchName,
                MiddleName = person.MiddleName.NullSafeTrim(),
                MotherId = person.MotherId.NullSafeTrim(),
                TreeId = person.ResultId.NullSafeTrim(),
            };
        }

        private PersonGender ParseGender(string gender)
        {
            if (string.IsNullOrWhiteSpace(gender))
            {
                return PersonGender.NotSet;
            }

            if (gender.Trim().ToUpper() == "F")
            {
                return PersonGender.Female;
            }

            if (gender.Trim().ToUpper() == "M")
            {
                return PersonGender.Male;
            }

            return PersonGender.Unknown;
        }

        private int? ParseGeneration(string generation)
        {
            if (string.IsNullOrWhiteSpace(generation))
            {
                return null;
            }

            if (int.TryParse(generation, out var parsedGeneration))
            {
                return parsedGeneration;
            }

            return null;
        }
    }
}
