// <copyright file="DateTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests.TreeAnalysis
{
    using System.Linq;
    using System.Threading.Tasks;
    using GeneGenie.DataQuality.Models;
    using GeneGenie.Sawmill.IO;
    using Xunit;

    public class DateTests
    {
        private readonly TreeAnalyser treeAnalyser;

        public DateTests()
        {
            treeAnalyser = new TreeAnalyser();
        }

        [Fact]
        public async Task Invalid_dates_raise_flag()
        {
            var jsonTreeWriter = new JsonTreeWriter(null, @"Data\\Processed tree.json");
            var trees = await jsonTreeWriter.ReadAllAsync();

            var treeStatistics = treeAnalyser.GenerateStatistics(trees);

            Assert.True(treeStatistics.DateIssuesExist);
        }

        [Fact]
        public async Task Date_breakdown_is_correct()
        {
            var jsonTreeWriter = new JsonTreeWriter(null, @"Data\\Processed tree.json");
            var trees = await jsonTreeWriter.ReadAllAsync();

            var treeStatistics = treeAnalyser.GenerateStatistics(trees);

            Assert.Equal(193, treeStatistics.DateCountsByStatus.Single(d => d.Status == DateQualityStatus.Empty).Count);
            Assert.Equal(115, treeStatistics.DateCountsByStatus.Single(d => d.Status == DateQualityStatus.MonthIsAmbiguous).Count);
            Assert.Equal(0, treeStatistics.DateCountsByStatus.Single(d => d.Status == DateQualityStatus.NotSet).Count);
            Assert.Equal(0, treeStatistics.DateCountsByStatus.Single(d => d.Status == DateQualityStatus.NotValid).Count);
            Assert.Equal(256, treeStatistics.DateCountsByStatus.Single(d => d.Status == DateQualityStatus.OK).Count);
            Assert.Equal(0, treeStatistics.DateCountsByStatus.Single(d => d.Status == DateQualityStatus.OnlyMonthIsPresent).Count);
            Assert.Equal(0, treeStatistics.DateCountsByStatus.Single(d => d.Status == DateQualityStatus.ThreePartsWithoutYear).Count);
            Assert.Equal(0, treeStatistics.DateCountsByStatus.Single(d => d.Status == DateQualityStatus.TooManyDateParts).Count);
            Assert.Equal(0, treeStatistics.DateCountsByStatus.Single(d => d.Status == DateQualityStatus.YearInMiddle).Count);
        }
    }
}
