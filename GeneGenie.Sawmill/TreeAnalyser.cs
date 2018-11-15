// <copyright file="TreeAnalyser.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GeneGenie.DataQuality.Models;
    using GeneGenie.Sawmill.Models;

    public class TreeAnalyser
    {
        public TreeStatistics GenerateStatistics(List<FamilyTree> trees)
        {
            var treeStatistics = new TreeStatistics();

            if (trees == null)
            {
                treeStatistics.TreesAreEmpty = true;
                return treeStatistics;
            }

            treeStatistics.TreesAreEmpty = !trees.Any();
            treeStatistics.NumberOfTrees = trees.Count;

            var people = trees.SelectMany(p => p.People).ToList();

            PopulateDateStatistics(treeStatistics, people);
            PopulateLocationStatistics(treeStatistics, people);

            return treeStatistics;
        }

        private static void PopulateDateStatistics(TreeStatistics treeStatistics, List<TreePerson> people)
        {
            treeStatistics.PeopleNotFound = !people.Any();
            treeStatistics.NumberOfPeople = people.Count;

            var allDates = people
                .Select(p => p.Birth.DateRange)
                .Concat(people.Select(p => p.Death.DateRange))
                .ToList();

            var dateStatusEnums = Enum.GetValues(typeof(DateQualityStatus));

            foreach (DateQualityStatus dateEnumStatus in dateStatusEnums)
            {
                var dateSummary = new DateSummary
                {
                    Count = allDates.Count(d => d.Status == dateEnumStatus),
                    Status = dateEnumStatus,
                };

                treeStatistics.DateCountsByStatus.Add(dateSummary);
            }

            treeStatistics.DateIssuesExist = treeStatistics
                .DateCountsByStatus
                .Where(d => d.Status != DateQualityStatus.OK)
                .Any(d => d.Count > 0);
        }

        private static void PopulateLocationStatistics(TreeStatistics treeStatistics, List<TreePerson> people)
        {
            var allLocations = people
                .Select(p => p.Birth.Location)
                .Concat(people.Select(p => p.Death.Location))
                .ToList();

            var locationStatusEnums = Enum.GetValues(typeof(SawmillStatus));

            foreach (SawmillStatus locationEnumStatus in locationStatusEnums)
            {
                var locationSummary = new LocationSummary
                {
                    Count = allLocations.Count(d => d.Status == locationEnumStatus),
                    Status = locationEnumStatus,
                };

                treeStatistics.LocationCountsByStatus.Add(locationSummary);
            }

            treeStatistics.LocationIssuesExist = treeStatistics
                .LocationCountsByStatus
                .Where(d => d.Status != SawmillStatus.Geocoded)
                .Any(d => d.Count > 0);
        }
    }
}
