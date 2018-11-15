// <copyright file="DateSummary.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Models
{
    using GeneGenie.DataQuality.Models;

    public class DateSummary
    {
        public int Count { get; set; }

        public DateQualityStatus Status { get; set; }
    }
}
