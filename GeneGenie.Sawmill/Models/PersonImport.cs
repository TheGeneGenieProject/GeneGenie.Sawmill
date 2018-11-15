// <copyright file="PersonImport.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Models
{
    /// <summary>
    /// Matched person data from the DNAGedcom utility.
    /// </summary>
    public class PersonImport
    {
        public string BirthDate { get; set; }

        public string BirthPlace { get; set; }

        public string ContactId { get; set; }

        public string DeathDate { get; set; }

        public string DeathPlace { get; set; }

        public string FatherId { get; set; }

        public string FirstName { get; set; }

        public string Generation { get; set; }

        public string Gender { get; set; }

        public string KitNumber { get; set; }

        public string LastName { get; set; }

        public string MatchName { get; set; }

        public string MiddleName { get; set; }

        public string MotherId { get; set; }

        /// <summary>
        /// Gets or sets the ResultId which is the id of the person who we are matched with.
        /// This can be used to group the results into a single family tree if there are multiple in the source.
        /// </summary>
        /// <value>An integer representing the unique ID of the person we match.</value>
        public string ResultId { get; set; }

        /// <summary>
        /// Gets or sets the TreeId which is badly named as it is the ID of the person in the tree,
        /// not the ID of the tree. It is effectively the unique ID of a person from a family tree
        /// and we match someone in that tree.
        /// </summary>
        /// <value>An integer representing the unique ID of the person in this tree.</value>
        public string TreeId { get; set; }
    }
}
