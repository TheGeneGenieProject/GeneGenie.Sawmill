// <copyright file="ConfigureDi.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests.Setup
{
    using System;
    using GeneGenie.Geocoder.Interfaces;
    using GeneGenie.Sawmill.Caching;
    using GeneGenie.Sawmill.ExtensionMethods;
    using GeneGenie.Sawmill.Interfaces;
    using GeneGenie.Sawmill.IO;
    using GeneGenie.Sawmill.Tests.Fakes;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;

    public class ConfigureDi
    {
        private static IServiceProvider services;

        public static IServiceProvider Services
        {
            get
            {
                if (services == null)
                {
                    services = BuildDi();
                }

                return services;
            }
        }

        public static IServiceProvider BuildDi()
        {
            var serviceProvider = new ServiceCollection()
                .AddSawmill()

                // Replace all real storage with fakes.
                .RemoveAll<ILocationCache>()
                .AddTransient<FakeLocationCacheFactory>()
                .AddTransient<FakeSawyerFactory>()

                .AddScoped<FakeLogger<CacheManager>>()
                .AddScoped<FakeLogger<LocationCacheMemory>>()
                .AddScoped<FakeLogger<LocationCacheDisk>>()
                .AddScoped<FakeLogger<CsvTreeReader>>()
                .AddScoped<FakeLogger<JsonTreeWriter>>()

                .AddLogging(opt =>
                {
                    opt.AddConsole();
                })

                // Replace the real geocoder lib with our fake, we're not testing that here (it belongs to another package).
                .RemoveAll<IGeocodeManager>()
                .AddTransient<IGeocodeManager, FakeGeocodeManager>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
