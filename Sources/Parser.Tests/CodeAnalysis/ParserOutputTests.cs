using NUnit.Framework;
using P3Model.Parser.CodeAnalysis;
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
        var output = new ParserOutput();
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
                .Use(output))
            .Logger(logger => logger
                .WriteTo.Debug()
                .MinimumLevel.Is(LogEventLevel.Verbose))
            .CreateRootAnalyzer();
        await Analyze(output, analyzer, TargetFramework.Net60);
        await Analyze(output, analyzer, TargetFramework.Net70);
        await Analyze(output, analyzer, TargetFramework.Net80);
    }

    private static async Task Analyze(ParserOutput output, RootAnalyzer analyzer, TargetFramework targetFramework)
    {
        output.SetTargetFramework(targetFramework);
        await analyzer.Analyze(targetFramework);
    }
}