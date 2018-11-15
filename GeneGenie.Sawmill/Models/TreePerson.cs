// <copyright file="TreePerson.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Models
{
    public class TreePerson
    {
        public LocationAndTime Birth { get; set; }

        public LocationAndTime Death { get; set; }

        public string FatherId { get; set; }

        public string FirstName { get; set; }

        public int? Generation { get; set; }

        public PersonGender Gender { get; set; }

        public string Id { get; set; }

        public string KitNumber { get; set; }

        public string LastName { get; set; }

        public string MatchName { get; set; }

        public string MiddleName { get; set; }

        public string MotherId { get; set; }

        public string TreeId { get; set; }
    }
}
