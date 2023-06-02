using Microsoft.Extensions.Configuration;
using P3Model.Parser.Configuration;

// This runner in only for initial tests of Parser POC !!!
// It's configured to work with "DDD starter for .net":  https://github.com/itlibrium/DDD-starter-dotnet.
// "DDD starter" uses P3 Model Annotations and has enough complexity to test Parser (in current POC version).
// You have to provide RepositoryPath and OutputPath that works on your machine in appsettings.Development.json.

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Development.json")
    .Build();
await P3
    .Repositories(repositories => repositories
        .Use(configuration["RepositoryPath"]!))
    .Analyzers(analyzers => analyzers
        .UseDefaults(options => options
            .TreatNamespacesAsDomainModules(namespaces => namespaces
                .Exclude("TechnicalStuff")
                .Exclude("Infrastructure")
                .Exclude("Adapters")
                .Exclude("RestApi")
                .Exclude("OldApi")
                .Exclude("Database")
                .Exclude("FluentMigrations")
                .Exclude("DI")
                .Exclude("Nuke")
                .RemoveRootNamespace("MyCompany.ECommerce"))))
    .OutputFormat(formatters => formatters
        .UseMermaid(options => options
            .Directory(configuration["OutputPath"]!)
            .UseDefaultPages()))
    .Analyze();