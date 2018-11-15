// <copyright file="ServiceCollectionExtensions.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.ExtensionMethods
{
    using System.Collections.Generic;
    using GeneGenie.DataQuality.ExtensionMethods;
    using GeneGenie.Geocoder.ExtensionMethods;
    using GeneGenie.Geocoder.Models;
    using GeneGenie.Sawmill.Caching;
    using GeneGenie.Sawmill.Interfaces;
    using GeneGenie.Sawmill.IO;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extension methods used for registering and resolving the services used by this library with the frameworks dependency injection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services for consuming this library from .Net for the processing tree data.
        /// </summary>
        /// <param name="serviceCollection">The service collection to add the registrations to.</param>
        /// <param name="geocoderSettings">An optional list of API keys to use with the geocoders. If not passed in here then you need to register
        /// these settings with your DI framework yourself.</param>
        /// <returns>The service collection with all sawmill classes registered.</returns>
        public static IServiceCollection AddSawmill(this IServiceCollection serviceCollection, List<GeocoderSettings> geocoderSettings = null)
        {
            return serviceCollection
                .AddDataQuality()
                .AddGeocoders(geocoderSettings)
                .AddLogging()
                .AddTransient<SawyerFactory>()
                .AddTransient<LocationCacheFactory>()
                .AddTransient<ILocationCacheFactory, LocationCacheFactory>()

                .AddTransient<CsvTreeReader>()
                .AddTransient<JsonTreeWriter>()

                .AddTransient<KeyComposer>()
                .AddTransient<LocationCreator>()
                .AddTransient<LocationFilter>()
                .AddScoped<LocationCacheMemory>()
                .AddScoped<LocationCacheDisk>()
                .AddTransient<TreeAnalyser>()
                .AddTransient<TreeParser>();
        }
    }
}
