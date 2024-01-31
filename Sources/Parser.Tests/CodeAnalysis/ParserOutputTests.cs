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
        var analyzer = await P3
            .Product(product => product
                .UseName("MyCompany MySystem"))
            .Repositories(repositories => repositories
                .Use("../../../../TestSamples/Main", repository => repository
                    .ExcludeProjects("Annotations"))
                .Use("../../../../TestSamples/StartupProjects", repository => repository
                    .ExcludeProjects("Annotations")))
            .Analyzers(analyzers => analyzers
                .UseDefaults(options => options
                    .TreatNamespacesAsDomainModules(namespaces => namespaces                        
                        .RemoveRootNamespace("TestSamples")
                        .RemoveNamespacePart("MainProject", "NotModule"))))
            .OutputFormat(formatters => formatters
                .Use(ParserOutput.Instance))
            .Logger(logger => logger
                .WriteTo.Debug()
                .MinimumLevel.Is(LogEventLevel.Verbose))
            .CreateRootAnalyzer();
        await analyzer.Analyze(TargetFramework.Net60);
        await analyzer.Analyze(TargetFramework.Net70);
        await analyzer.Analyze(TargetFramework.Net80);
    }
}