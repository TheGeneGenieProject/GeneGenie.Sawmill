// <copyright file="ConsoleTreeReporter.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Console.Logic
{
    using System;
    using System.Linq;
    using GeneGenie.DataQuality.Models;
    using GeneGenie.Sawmill.Models;

    public class ConsoleTreeReporter
    {
        public void OutputToConsole(TreeStatistics treeStatistics)
        {
            var originalColour = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("GeneGenie.Sawmill.Console - report on status of processed trees.");
            Console.WriteLine();

            Console.Write("{0,-25}", "Number of trees");
            Console.WriteLine($"{treeStatistics.NumberOfTrees,5}");
            Console.Write("{0,-25}", "Number of people");
            Console.WriteLine($"{treeStatistics.NumberOfPeople,5}");

            if (treeStatistics.TreesAreEmpty)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"No trees were found.");
            }
            else if (treeStatistics.PeopleNotFound)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"No people were found in any of the trees.");
            }

            OutputDates(treeStatistics);
            OutputLocations(treeStatistics);
            Console.WriteLine();

            Console.ForegroundColor = originalColour;
        }

        private static void OutputDates(TreeStatistics treeStatistics)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"**** Dates ****");
            foreach (var dateSummary in treeStatistics.DateCountsByStatus)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{dateSummary.Status,-25}");

                Console.ForegroundColor = ColourForDateSummary(dateSummary);
                Console.WriteLine($"{dateSummary.Count,5}");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("{0,-25}", "Total");
            Console.WriteLine($"{treeStatistics.DateCountsByStatus.Sum(l => l.Count),5}");
            Console.WriteLine($"**** Dates ****");
        }

        private static ConsoleColor ColourForDateSummary(DateSummary dateSummary)
        {
            if (dateSummary.Status == DateQualityStatus.OK && dateSummary.Count == 0)
            {
                return ConsoleColor.Yellow;
            }
            else if (dateSummary.Status == DateQualityStatus.MonthIsAmbiguous && dateSummary.Count > 0)
            {
                return ConsoleColor.Yellow;
            }
            else if (dateSummary.Status == DateQualityStatus.Empty && dateSummary.Count > 0)
            {
                return ConsoleColor.Yellow;
            }
            else if (dateSummary.Status != DateQualityStatus.OK && dateSummary.Count > 0)
            {
                return ConsoleColor.Red;
            }

            return ConsoleColor.Green;
        }

        private static void OutputLocations(TreeStatistics treeStatistics)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"**** Locations ****");
            foreach (var locationSummary in treeStatistics.LocationCountsByStatus)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{locationSummary.Status,-25}");

                Console.ForegroundColor = ColourForLocationSummary(locationSummary);
                Console.WriteLine($"{locationSummary.Count,5}");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("{0,-25}", "Total");
            Console.WriteLine($"{treeStatistics.LocationCountsByStatus.Sum(l => l.Count),5}");
            Console.WriteLine($"**** Locations ****");
        }

        private static ConsoleColor ColourForLocationSummary(LocationSummary locationSummary)
        {
            if (locationSummary.Status == SawmillStatus.Geocoded && locationSummary.Count == 0)
            {
                return ConsoleColor.Yellow;
            }
            else if (locationSummary.Status == SawmillStatus.SkippedAsEmpty && locationSummary.Count > 0)
            {
                return ConsoleColor.Yellow;
            }
            else if (locationSummary.Status != SawmillStatus.Geocoded && locationSummary.Count > 0)
            {
                return ConsoleColor.Red;
            }

            return ConsoleColor.Green;
        }
    }
}
