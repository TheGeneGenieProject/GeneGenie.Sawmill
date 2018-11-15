// <copyright file="CsvTreeReaderMap.cs" company="GeneGenie.com">
// Copyright (c) GeneGenie.com. All Rights Reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE in the project root for license information.
// </copyright>

namespace GeneGenie.Sawmill.IO
{
    using CsvHelper.Configuration;
    using GeneGenie.Sawmill.Models;

    public class CsvTreeReaderMap : ClassMap<PersonImport>
    {
        public CsvTreeReaderMap()
        {
            Map(m => m.BirthDate).Name("Birth Date");
            Map(m => m.BirthPlace).Name("Birth Place");
            Map(m => m.ContactId).Name("ContactId");
            Map(m => m.DeathDate).Name("Death Date");
            Map(m => m.DeathPlace).Name("Death Place");
            Map(m => m.FatherId).Name("Father Id");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.Generation).Name("Generation");
            Map(m => m.Gender).Name("Gender");
            Map(m => m.KitNumber).Name("Kit Number");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.MatchName).Name("Match Name");
            Map(m => m.MiddleName).Name("Middle Name");
            Map(m => m.MotherId).Name("Mother Id");
            Map(m => m.ResultId).Name("Resultid");
            Map(m => m.TreeId).Name("TreeId");
        }
    }
}