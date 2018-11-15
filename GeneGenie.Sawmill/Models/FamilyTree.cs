// <copyright file="FamilyTree.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Models
{
    using System.Collections.Generic;

    public class FamilyTree
    {
        public string MatchName { get; set; }

        public List<TreePerson> People { get; set; }

        public string TreeId { get; set; }
    }
}
