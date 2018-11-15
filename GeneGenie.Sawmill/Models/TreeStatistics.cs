// <copyright file="TreeStatistics.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Models
{
    using System.Collections.Generic;

    public class TreeStatistics
    {
        public List<DateSummary> DateCountsByStatus { get; } = new List<DateSummary>();

        public bool DateIssuesExist { get; set; }

        public List<LocationSummary> LocationCountsByStatus { get; } = new List<LocationSummary>();

        public bool LocationIssuesExist { get; set; }

        public int NumberOfPeople { get; set; }

        public int NumberOfTrees { get; set; }

        public bool PeopleNotFound { get; set; }

        /// <summary>
        /// The list of passed trees is empty, the source data is somehow wrong.
        /// </summary>
        public bool TreesAreEmpty { get; set; }
    }
}
