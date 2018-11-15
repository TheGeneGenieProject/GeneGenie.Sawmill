// <copyright file="FakeTreeWriter.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests.Fakes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Interfaces;
    using GeneGenie.Sawmill.Models;

    public class FakeTreeWriter : ITreeWriter
    {
        public List<FamilyTree> Trees { get; } = new List<FamilyTree>();

        public async Task<List<FamilyTree>> ReadAllAsync()
        {
            return await Task.FromResult(Trees);
        }

        public async Task<bool> TargetExistsAsync()
        {
            return await Task.FromResult(Trees.Any());
        }

        public async Task WriteAllAsync(List<FamilyTree> trees)
        {
            Trees.AddRange(trees);
            await Task.CompletedTask;
        }
    }
}
