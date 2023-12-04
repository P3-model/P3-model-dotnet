using NUnit.Framework;
using P3Model.Annotations.Domain;
using P3Model.Parser.Configuration;

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
                .Use("../../../../TestSamples/Net60/Repository1")
                .Use("../../../../TestSamples/Net60/Repository2"))
            .Analyzers(analyzers => analyzers
                .UseDefaults(options => options
                    .TreatNamespacesAsDomainModules(namespaces => namespaces
                        .OnlyFromAssembliesAnnotatedWith<DomainModelAttribute>()
                        .RemoveRootNamespace("MyCompany.MySystem")
                        .RemoveNamespacePart("Api", "BusinessLogic", "Infrastructure", "Repositories", "Entities",
                            "Model", "Models", "Controllers"))))
            .OutputFormat(formatters => formatters
                .Use(parserOutput))
            .Analyze();
    }

    [OneTimeTearDown]
    public void AllGeneratedElementsAreChecked() => ParserOutput.AssertAllElementsAreChecked();
}