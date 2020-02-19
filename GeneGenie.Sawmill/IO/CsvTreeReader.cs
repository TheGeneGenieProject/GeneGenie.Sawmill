// <copyright file="CsvTreeReader.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Interfaces;
    using GeneGenie.Sawmill.Models;
    using Microsoft.Extensions.Logging;

    public class CsvTreeReader : ITreeReader
    {
        private readonly string pathToCsvFile;
        private readonly ILogger<CsvTreeReader> logger;

        public CsvTreeReader(string pathToCsvFile, ILogger<CsvTreeReader> logger)
        {
            this.pathToCsvFile = pathToCsvFile;
            this.logger = logger;
        }

        public async Task<List<PersonImport>> ReadAllAsync()
        {
            var people = new List<PersonImport>();

            if (string.IsNullOrWhiteSpace(pathToCsvFile) || !File.Exists(pathToCsvFile))
            {
                return people;
            }

            using (var sr = new StreamReader(pathToCsvFile))
            {
                using (var csv = new CsvHelper.CsvReader(sr, System.Globalization.CultureInfo.InvariantCulture))
                {
                    csv.Configuration.RegisterClassMap<CsvTreeReaderMap>();
                    csv.Configuration.BadDataFound = context =>
                    {
                        logger?.LogError((int)LogEventIds.BadDataInReaderImport, $"Bad data found on row '{context.RawRow}'.");
                    };

                    while (await csv.ReadAsync())
                    {
                        try
                        {
                            var person = csv.GetRecord<PersonImport>();
                            people.Add(person);
                        }
                        catch (Exception ex)
                        {
                            logger?.LogError((int)LogEventIds.ErrorReadingCsvInReader, ex, $"Error occurred reading CSV.");
                        }
                    }
                }
            }

            return people;
        }
    }
}