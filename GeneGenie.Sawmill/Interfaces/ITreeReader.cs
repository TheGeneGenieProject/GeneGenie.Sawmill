// <copyright file="ITreeReader.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Models;

    /// <summary>
    /// Defines an interface for reading tree data. Storage implementations use this to
    /// deal with different types of storage (CSV on disk, JSON in Azure, SQL etc.).
    /// </summary>
    public interface ITreeReader
    {
        Task<List<PersonImport>> ReadAllAsync();
    }
}
