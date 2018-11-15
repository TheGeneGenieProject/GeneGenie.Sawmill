// <copyright file="Program.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Console
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using GeneGenie.Sawmill.Console.Logic;
    using GeneGenie.Sawmill.Console.Setup;
    using GeneGenie.Sawmill.IO;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        private static IServiceProvider serviceProvider;
        private static ILogger<Program> logger;

        public static async Task Main(string[] args)
        {
            var configuration = ConfigureSettings.Build(args);
            serviceProvider = ConfigureDi.BuildDi(configuration);
            logger = serviceProvider.GetService<ILogger<Program>>();

            try
            {
                logger.LogInformation("Starting");

                var configurationChecker = serviceProvider.GetRequiredService<ConfigurationChecker>();
                var configurationCheck = configurationChecker.ConfigurationIsValid();
                if (configurationCheck.Errors.Any())
                {
                    logger.LogError("Configuration is invalid, stopping.");
                    return;
                }

                var argumentParser = serviceProvider.GetRequiredService<ArgumentParser>();
                var dataFiles = argumentParser.ParseArgs(args);

                if (dataFiles.UnableToParseArguments)
                {
                    logger.LogError("Unable to parse command line, details follow.");
                    foreach (var message in dataFiles.Errors)
                    {
                        logger.LogCritical(message);
                    }
                }
                else
                {
                    var sawyerFactory = serviceProvider.GetRequiredService<SawyerFactory>();
                    var sawmill = sawyerFactory.CreateCsvReaderWriter(dataFiles.Source.FullName, dataFiles.Destination.FullName);
                    await sawmill.ProcessTreeDataAsync();

                    // Output a bunch of stats on the tree we just processed.
                    var treeAnalyser = serviceProvider.GetRequiredService<TreeAnalyser>();
                    var jsonTreeWriter = new JsonTreeWriter(null, dataFiles.Destination.FullName);
                    var trees = await jsonTreeWriter.ReadAllAsync();
                    var treeStatistics = treeAnalyser.GenerateStatistics(trees);
                    var consoleTreeReporter = serviceProvider.GetRequiredService<ConsoleTreeReporter>();

                    consoleTreeReporter.OutputToConsole(treeStatistics);
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Unable to start application.");
                Console.WriteLine("Unable to start application.");
            }

            logger.LogInformation("Complete");
            Console.WriteLine("Press a key to exit.");
            Console.ReadKey();
        }
    }
}
