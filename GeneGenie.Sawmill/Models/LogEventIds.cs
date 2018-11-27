// <copyright file="LogEventIds.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Models
{
    /// <summary>
    /// Events logged by the Sawmill project to the standard logging framework.
    /// </summary>
    public enum LogEventIds
    {
        /// <summary>
        /// There should never be an event associated with this ID, if there is, some code has not set the status correctly.
        /// </summary>
        NotSet = 0,

        L1CacheManagerHit = 1,

        L2CacheManagerHit = 2,

        L1CacheManagerMiss = 3,

        L2CacheManagerMiss = 4,

        L2DiskCacheSearching = 5,

        L2DiskCacheFound = 6,

        L2DiskCacheNotFound = 7,

        L2DiskCacheInserted = 8,

        L2DiskCacheAlreadyExists = 9,

        L1MemoryCacheSearching = 10,

        L1MemoryCacheFound = 11,

        L1MemoryCacheNotFound = 12,

        L1MemoryCacheInserted = 13,

        L1MemoryCacheAlreadyExists = 14,

        BadDataInReaderImport = 15,

        ErrorReadingCsvInReader = 16,

        TreeWriterReadingFile = 17,

        BadDataInWriterImport = 18,

        ErrorReadingCsvInWriter = 19,

        GeocodingLocations = 20,

        // CheckingTree = 21,

        CheckingCache = 22,
    }
}
