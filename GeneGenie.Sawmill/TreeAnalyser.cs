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
        public TreeStatistics GenerateStatistics(List<WhoWhatWhereWhen> whoWhatWhereWhen)
        {
            var treeStatistics = new TreeStatistics();

            if (whoWhatWhereWhen == null)
            {
                treeStatistics.TreesAreEmpty = true;
                return treeStatistics;
            }

            treeStatistics.TreesAreEmpty = !whoWhatWhereWhen.Any();
            treeStatistics.NumberOfTrees = whoWhatWhereWhen.GroupBy(g => g.Who.TreeId).Count();

            PopulateDateStatistics(treeStatistics, whoWhatWhereWhen);
            PopulateLocationStatistics(treeStatistics, whoWhatWhereWhen);

            return treeStatistics;
        }

        private static void PopulateDateStatistics(TreeStatistics treeStatistics, List<WhoWhatWhereWhen> whoWhatWhereWhen)
        {
            var peopleGrouped = whoWhatWhereWhen
                .GroupBy(g => g.Who.Id)
                .ToList();

            treeStatistics.PeopleNotFound = !peopleGrouped.Any();
            treeStatistics.NumberOfPeople = peopleGrouped.Count();

            var allDates = whoWhatWhereWhen
                .Select(p => p.When.DateRange)
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

        private static void PopulateLocationStatistics(TreeStatistics treeStatistics, List<WhoWhatWhereWhen> whoWhatWhereWhen)
        {
            var allLocations = whoWhatWhereWhen
                .Select(p => p.Where.Location)
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
