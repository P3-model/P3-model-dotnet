using NUnit.Framework;
using P3Model.Annotations.Domain;
using P3Model.Parser.Configuration;
using Serilog;
using Serilog.Events;

namespace P3Model.Parser.Tests.CodeAnalysis;

[SetUpFixture]
public class ParserOutputTests
{
    [OneTimeSetUp]
    public async Task CreateModel()
    {
        var parserOutput = new ParserOutput();
        await P3
            .Product(product => product
                .UseName("MyCompany MySystem"))
            .Repositories(repositories => repositories
                .Use("../../../../TestSamples/Net60/Repository1", repository => repository
                    .ExcludeProjects("Annotations"))
                .Use("../../../../TestSamples/Net60/Repository2", repository => repository
                    .ExcludeProjects("Annotations"))
                .Use("../../../../TestSamples/Net70", repository => repository
                    .ExcludeProjects("Annotations")))
            .Analyzers(analyzers => analyzers
                .UseDefaults(options => options
                    .TreatNamespacesAsDomainModules(namespaces => namespaces
                        .OnlyFromAssembliesAnnotatedWith<DomainModelAttribute>()
                        .RemoveRootNamespace("MyCompany.MySystem")
                        .RemoveNamespacePart("Api", "BusinessLogic", "Infrastructure", "Repositories", "Entities",
                            "Controllers"))))
            .OutputFormat(formatters => formatters
                .Use(parserOutput))
            .Logger(logger => logger
                .WriteTo.Debug()
                .MinimumLevel.Is(LogEventLevel.Verbose))
            .Analyze();
    }
}