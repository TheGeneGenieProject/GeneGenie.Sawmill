// <copyright file="InstantiationTests.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    /// <summary>
    /// Tests to check that the <see cref="Sawyer"/> class can be constructed without error.
    /// </summary>
    public class InstantiationTests
    {
        private readonly SawyerFactory sawyerFactory;

        public InstantiationTests()
        {
            var serviceProvider = Setup.ConfigureDi.Services;
            sawyerFactory = serviceProvider.GetRequiredService<SawyerFactory>();
        }

        /// <summary>
        /// Checks that <see cref="Sawyer"/> can be created from <see cref="SawyerFactory"/>.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void Sawyer_can_be_created_from_factory()
        {
            sawyerFactory.CreateCsvReaderWriter(string.Empty, string.Empty);
        }
    }
}
