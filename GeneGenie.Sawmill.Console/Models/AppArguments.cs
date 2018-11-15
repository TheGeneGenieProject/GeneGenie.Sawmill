// <copyright file="AppArguments.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Console.Models
{
    using System.Collections.Generic;
    using System.IO;

    public class AppArguments
    {
        public FileInfo Destination { get; set; }

        public List<string> Errors { get; } = new List<string>();

        public FileInfo Source { get; set; }

        public bool UnableToParseArguments { get; set; }
    }
}
