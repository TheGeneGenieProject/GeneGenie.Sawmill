﻿// <copyright file="JsonTreeWriter.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.IO
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Interfaces;
    using GeneGenie.Sawmill.Models;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public class JsonTreeWriter : ITreeWriter
    {
        private readonly ILogger<JsonTreeWriter> logger;
        private readonly string pathToFile;

        public JsonTreeWriter(ILogger<JsonTreeWriter> logger, string pathToFile)
        {
            this.logger = logger;
            this.pathToFile = pathToFile;
        }

        public async Task<List<WhoWhatWhereWhen>> ReadAllAsync()
        {
            var whoWhatWhereWhen = new List<WhoWhatWhereWhen>();

            if (string.IsNullOrWhiteSpace(pathToFile) || !(await TargetExistsAsync()))
            {
                return whoWhatWhereWhen;
            }

            logger?.LogInformation((int)LogEventIds.TreeWriterReadingFile, "Reading '{pathToFile}'.", pathToFile);
            using (var sr = new StreamReader(pathToFile))
            {
                var json = await sr.ReadToEndAsync();

                return JsonConvert.DeserializeObject<List<WhoWhatWhereWhen>>(json);
            }
        }

        public async Task<bool> TargetExistsAsync()
        {
            return await Task.FromResult(File.Exists(pathToFile));
        }

        public async Task WriteAllAsync(List<WhoWhatWhereWhen> whoWhatWhereWhen)
        {
            var json = JsonConvert.SerializeObject(whoWhatWhereWhen, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented });

            using (var sw = new StreamWriter(pathToFile))
            {
                await sw.WriteAsync(json);
            }
        }
    }
}
