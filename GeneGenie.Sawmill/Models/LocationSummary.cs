// <copyright file="LocationSummary.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class LocationSummary
    {
        public int Count { get; set; }

        public SawmillStatus Status { get; set; }
    }
}
