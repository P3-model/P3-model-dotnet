using Microsoft.Extensions.Configuration;
using P3Model.Annotations.Domain;
using P3Model.Parser.Configuration;

// This runner in only for initial tests of Parser POC !!!
// It's configured to work with "DDD starter for .net":  https://github.com/itlibrium/DDD-starter-dotnet.
// "DDD starter" uses P3 Model Annotations and has enough complexity to test Parser (in current POC version).
// You have to provide RepositoryPath and OutputPath that works on your machine in appsettings.Development.json.

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Development.json")
    .Build();
await P3
    .Product(product => product
        .UseName("MyCompany e-commerce"))
    .Repositories(repositories => repositories
        .Use(configuration["RepositoryPath"]!))
    .Analyzers(analyzers => analyzers
        .UseDefaults(options => options
            .TreatNamespacesAsDomainModules(namespaces => namespaces
                .OnlyFromAssembliesAnnotatedWith<DomainModelAttribute>()
                .RemoveRootNamespace("MyCompany.ECommerce"))))
    .OutputFormat(formatters => formatters
        .UseMermaid(options => options
            .Directory($"{configuration["OutputPath"]!}/MermaidOutput")
            .UseDefaultPages())
        .UseJson(options => options
            .File($"{configuration["OutputPath"]!}/JsonOutput/model.json")))
    .Analyze();