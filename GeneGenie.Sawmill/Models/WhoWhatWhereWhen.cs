// <copyright file="WhoWhatWhereWhen.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Models
{
    /// <summary>
    /// Defines the event properties required to plot an event in 3d space.
    /// </summary>
    public class WhoWhatWhereWhen
    {
        /// <summary>Gets or sets an instance of <see cref="What"/> that indicates what occurred.</summary>
        public What What { get; set; }

        /// <summary>Gets or sets an instance of <see cref="When"/> that indicates when this event occurred.</summary>
        public When When { get; set; }

        /// <summary>Gets or sets an instance of <see cref="Where"/> that indicates where this event occurred.</summary>
        public Where Where { get; set; }

        /// <summary>Gets or sets an instance of <see cref="Who"/> that indicates who this event happened to.</summary>
        public Who Who { get; set; }
    }
}
