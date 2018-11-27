// <copyright file="When.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Models
{
    using System;
    using GeneGenie.DataQuality.Models;

    /// <summary>
    /// Details when an event occurs for plotting on a 3d chart.
    /// </summary>
    public class When
    {
        /// <summary>Gets or sets the date range for this event.</summary>
        public DateRange DateRange { get; set; }

        ///// <summary>Gets or sets the label that is shown to the user for this event.</summary>
        // public string Label { get; set; }
    }
}
