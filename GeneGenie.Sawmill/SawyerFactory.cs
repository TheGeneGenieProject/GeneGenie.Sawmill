// <copyright file="SawyerFactory.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill
{
    using System;
    using GeneGenie.DataQuality;
    using GeneGenie.Geocoder.Interfaces;
    using GeneGenie.Sawmill.Interfaces;
    using GeneGenie.Sawmill.IO;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class SawyerFactory
    {
        private readonly IServiceProvider serviceProvider;

        public SawyerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Sawyer CreateCsvReaderWriter(string source, string destination)
        {
            var addressQualityChecker = serviceProvider.GetRequiredService<AddressQualityChecker>();
            var readerLogger = serviceProvider.GetRequiredService<ILogger<CsvTreeReader>>();
            var reader = new CsvTreeReader(source, readerLogger);
            var writerLogger = serviceProvider.GetRequiredService<ILogger<JsonTreeWriter>>();
            var writer = new JsonTreeWriter(writerLogger, destination);

            var geocodeManager = serviceProvider.GetRequiredService<IGeocodeManager>();
            var locationCacheFactory = serviceProvider.GetRequiredService<ILocationCacheFactory>();
            var logger = serviceProvider.GetRequiredService<ILogger<Sawyer>>();
            var treeParser = serviceProvider.GetRequiredService<TreeParser>();

            return new Sawyer(addressQualityChecker, geocodeManager, locationCacheFactory, logger, treeParser, reader, writer);
        }
    }
}
