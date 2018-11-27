// <copyright file="ITreeWriter.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Models;

    /// <summary>
    /// Defines an interface for writing tree data as a set of information ready to be plotted
    /// in 3d space. Storage implementations use this to
    /// deal with different types of storage (CSV on disk, JSON in Azure, SQL etc.).
    /// Implementors need to provide methods for reading as well
    /// so that the current status of the tree in storage can be calculated (it may only be partially
    /// processed with records in a failed state).
    /// </summary>
    public interface ITreeWriter
    {
        Task<List<WhoWhatWhereWhen>> ReadAllAsync();

        Task<bool> TargetExistsAsync();

        Task WriteAllAsync(List<WhoWhatWhereWhen> whoWhatWhereWhen);
    }
}
