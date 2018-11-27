// <copyright file="Where.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Models
{
    /// <summary>
    /// Details where an event occurs for plotting on a 3d chart.
    /// </summary>
    public class Where
    {
        /// <summary>Gets or sets a label for this location that can be shown to the user.</summary>
        public string Label { get; set; }

        /// <summary>Gets or sets the location of this event.</summary>
        public SawmillGeocodeRequest Location { get; set; }

        ///// <summary>Gets or sets the latitude (chart z axis) for this event.</summary>
        //public decimal Latitude { get; set; }

        ///// <summary>Gets or sets the longitude (chart x axis) for this event.</summary>
        //public decimal Longitude { get; set; }

        ///// <summary>Gets or sets the radius of the event in metres.</summary>
        //public decimal Radius { get; set; }
    }
}
