# GeneGenie.Sawmill

So named because it takes family trees and chops them up into a more refined product. Probably only of use to you if you are looking at the internal workings of GeneGenie or want to add new data sources.

These are tools to convert genealogical data from user entered addresses and dates to precise coordinates and date ranges for use within the GeneGenie system.

Consisting of a .Net Standard library (GeneGenie.Sawmill) and a console app (GeneGenie.Sawmill.Console) that uses the library to transform the data you provide.

The source data should be provided in a CSV format or you can write your own source adaptor if you have a different shape or format data source.

The output of the console app is a JSON formatted document too although you can again write your own output adaptors if you want to put the data somewhere else.

Depending on the quality of the data you are transforming it may take multiple passes to complete. For example, you may need to edit known bad data out of the address fields or correct spelling errors where the geocoder services cannot make sense of the address. Also, people enter a lot of junk into date fields although we'll do our best to figure out what was meant!

## Build status
[![AppVeyor branch](https://img.shields.io/appveyor/ci/RyanONeill1970/genegenie-sawmill/master.svg)](https://ci.appveyor.com/project/RyanONeill1970/genegenie-sawmill) [![NuGet](https://img.shields.io/nuget/v/GeneGenie.Sawmill.svg)](https://www.nuget.org/packages/GeneGenie.Sawmill) [![AppVeyor tests](https://img.shields.io/appveyor/tests/RyanONeill1970/genegenie-sawmill.svg)](https://ci.appveyor.com/project/RyanONeill1970/genegenie-sawmill/build/tests)

### Code quality
[![Maintainability](https://sonarcloud.io/api/project_badges/measure?project=GeneGenie.Sawmill&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=GeneGenie.Sawmill) [![Quality gate](https://sonarcloud.io/api/project_badges/measure?project=GeneGenie.Sawmill&metric=alert_status)](https://sonarcloud.io/dashboard?id=GeneGenie.Sawmill) [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=GeneGenie.Sawmill&metric=bugs)](https://sonarcloud.io/component_measures?id=GeneGenie.Sawmill&metric=Reliability) [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=GeneGenie.Sawmill&metric=vulnerabilities)](https://sonarcloud.io/component_measures?id=GeneGenie.Sawmill&metric=Security) [![Code smells](https://sonarcloud.io/api/project_badges/measure?project=GeneGenie.Sawmill&metric=code_smells)](https://sonarcloud.io/component_measures?id=GeneGenie.Sawmill&metric=Maintainability) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=GeneGenie.Sawmill&metric=coverage)](https://sonarcloud.io/component_measures?id=GeneGenie.Sawmill&metric=Coverage) [![Duplications](https://sonarcloud.io/api/project_badges/measure?project=GeneGenie.Sawmill&metric=duplicated_lines_density)](https://sonarcloud.io/component_measures?id=GeneGenie.Sawmill&metric=Duplications) [![Reliability](https://sonarcloud.io/api/project_badges/measure?project=GeneGenie.Sawmill&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=GeneGenie.Sawmill) [![Security](https://sonarcloud.io/api/project_badges/measure?project=GeneGenie.Sawmill&metric=security_rating)](https://sonarcloud.io/dashboard?id=GeneGenie.Sawmill) [![Security](https://sonarcloud.io/api/project_badges/measure?project=GeneGenie.Sawmill&metric=sqale_index)](https://sonarcloud.io/dashboard?id=GeneGenie.Sawmill) [![Lines of code](https://sonarcloud.io/api/project_badges/measure?project=GeneGenie.Sawmill&metric=ncloc)](https://sonarcloud.io/dashboard?id=GeneGenie.Sawmill)

[![Build stats](https://buildstats.info/appveyor/chart/ryanoneill1970/genegenie-sawmill)](https://ci.appveyor.com/project/ryanoneill1970/genegenie-sawmill/history)

## Contributing

We would love your help, subject to a few points. See [Contributing.md](Contributing.md) for guidelines.
