// <copyright file="Who.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Models
{
    /// <summary>
    /// Summary information on who an event refers to.
    /// </summary>
    public class Who
    {
        /// <summary>Gets or sets the unique id of the person which can be used to look up more detail.</summary>
        public string Id { get; set; }

        /// <summary>Gets or sets a label for the person to present to the user. This will typically be their name.</summary>
        public string Label { get; set; }

        /// <summary>Gets or sets the id of the tree which a person belongs to.</summary>
        public string TreeId { get; set; }
        public string FatherId { get; internal set; }
        public string FirstName { get; internal set; }
        public PersonGender Gender { get; internal set; }
        public int? Generation { get; internal set; }
        public string LastName { get; internal set; }
        public string MatchName { get; internal set; }
        public string MiddleName { get; internal set; }
        public string MotherId { get; internal set; }
    }
}
