// <copyright file="What.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Models
{
    /// <summary>Describes what type of event has occurred.</summary>
    public class What
    {
        ///// <summary>Gets or sets the unique id of the event which can be used to look up more detail.</summary>
        // public string Id { get; set; }

        /// <summary>Gets or sets the type of event that has occurred.</summary>
        public PersonEventType EventType { get; set; }

        ///// <summary>Gets or sets a label saying what the event is, used for displaying to the user.</summary>
        // public string Label { get; set; }
    }
}
