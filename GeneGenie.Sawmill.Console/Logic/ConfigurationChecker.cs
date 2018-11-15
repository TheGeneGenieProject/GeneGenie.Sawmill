// <copyright file="ConfigurationChecker.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Console.Logic
{
    using System.Linq;
    using GeneGenie.Sawmill.Console.Models;

    public class ConfigurationChecker
    {
        private readonly AppSettings appSettings;

        public ConfigurationChecker(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        internal ConfigurationCheck ConfigurationIsValid()
        {
            var configurationCheck = new ConfigurationCheck();

            if (appSettings.GeocoderSettings == null || !appSettings.GeocoderSettings.Any())
            {
                configurationCheck.Errors.Add("There are no geocoder settings present in the configuration, please check.");
                return configurationCheck;
            }

            foreach (var geocoderSetting in appSettings.GeocoderSettings)
            {
                if (string.IsNullOrWhiteSpace(geocoderSetting.ApiKey))
                {
                    configurationCheck.Errors.Add($"The geocoder API key for {geocoderSetting.GeocoderName} is blank and cannot be used.");
                }
                else if (!LooksLikeAnApiKey(geocoderSetting.ApiKey))
                {
                    configurationCheck.Errors.Add($"The geocoder API key {geocoderSetting.ApiKey} for {geocoderSetting.GeocoderName} does not look valid.");
                }
            }

            return configurationCheck;
        }

        private bool LooksLikeAnApiKey(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            return text.All(c => char.IsLetterOrDigit(c) || c == '-');
        }
    }
}
