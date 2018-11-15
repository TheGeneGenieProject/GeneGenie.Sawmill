// <copyright file="ArgumentParser.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Console.Logic
{
    using System.IO;
    using System.Linq;
    using GeneGenie.Sawmill.Console.Models;

    public class ArgumentParser
    {
        internal AppArguments ParseArgs(string[] args)
        {
            var appArguments = new AppArguments();

            if (args == null)
            {
                appArguments.Errors.Add("Expected 2 arguments but did not receive any.");
            }
            else if (args.Length != 2)
            {
                appArguments.Errors.Add($"Expected 2 arguments but received {args.Length}.");
            }
            else
            {
                appArguments.Source = new FileInfo(args[0]);
                if (!appArguments.Source.Exists)
                {
                    appArguments.Errors.Add($"The source file '{appArguments.Source.FullName}' does not exist, nothing to read from so quitting.");
                }

                appArguments.Destination = new FileInfo(args[1]);
            }

            appArguments.UnableToParseArguments = appArguments.Errors.Any();

            return appArguments;
        }
    }
}
