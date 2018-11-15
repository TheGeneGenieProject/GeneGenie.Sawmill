// <copyright file="FakeSawyerFactory.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests.Fakes
{
    using System;
    using GeneGenie.DataQuality;
    using GeneGenie.Sawmill.IO;
    using Microsoft.Extensions.DependencyInjection;

    public class FakeSawyerFactory
    {
        private readonly IServiceProvider serviceProvider;

        public FakeSawyerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public FakeGeocodeManager GeocodeManager { get; private set; }

        public FakeLocationCacheFactory LocationCacheFactory { get; private set; }

        public TreeParser TreeParser { get; private set; }

        public FakeTreeReader TreeReader { get; private set; }

        public FakeTreeWriter TreeWriter { get; private set; }

        public FakeLogger<CsvTreeReader> ReaderLogger { get; private set; }

        public FakeLogger<JsonTreeWriter> WriterLogger { get; private set; }

        /// <summary>
        /// Creates an instance of <see cref="Sawyer"/> that is wired up with fake I/O classes for unit testing.
        /// </summary>
        /// <returns></returns>
        public Sawyer Create()
        {
            var addressQualityChecker = serviceProvider.GetRequiredService<AddressQualityChecker>();
            GeocodeManager = new FakeGeocodeManager();
            LocationCacheFactory = serviceProvider.GetRequiredService<FakeLocationCacheFactory>();
            TreeParser = serviceProvider.GetRequiredService<TreeParser>();
            TreeReader = new FakeTreeReader();
            TreeWriter = new FakeTreeWriter();
            return new Sawyer(addressQualityChecker, GeocodeManager, LocationCacheFactory, null, TreeParser, TreeReader, TreeWriter);
        }

        /// <summary>
        /// Creates an instance of <see cref="Sawyer"/> that is wired up to real I/O but the cache and geocoders are all fake I/O classes for unit testing.
        /// </summary>
        /// <returns></returns>
        public Sawyer CreateWithLocalIO(string source, string destination)
        {
            var addressQualityChecker = serviceProvider.GetRequiredService<AddressQualityChecker>();
            ReaderLogger = serviceProvider.GetRequiredService<FakeLogger<CsvTreeReader>>();
            var reader = new CsvTreeReader(source, ReaderLogger);
            WriterLogger = serviceProvider.GetRequiredService<FakeLogger<JsonTreeWriter>>();
            var writer = new JsonTreeWriter(WriterLogger, destination);

            GeocodeManager = new FakeGeocodeManager();
            LocationCacheFactory = serviceProvider.GetRequiredService<FakeLocationCacheFactory>();
            TreeParser = serviceProvider.GetRequiredService<TreeParser>();

            return new Sawyer(addressQualityChecker, GeocodeManager, LocationCacheFactory, null, TreeParser, reader, writer);
        }
    }
}
