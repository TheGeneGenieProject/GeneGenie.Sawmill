// <copyright file="ILocationCache.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Interfaces
{
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Models;

    public interface ILocationCache
    {
        Task InsertAsync(SawmillGeocodeRequest location);

        Task<SawmillGeocodeRequest> FindByKeyAsync(string sourceKey);
    }
}
