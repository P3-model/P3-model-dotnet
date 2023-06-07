using System;
using System.Threading.Tasks;
using P3Model.Parser.CodeAnalysis;
using P3Model.Parser.Configuration.Analyzers;
using P3Model.Parser.Configuration.OutputFormat;
using P3Model.Parser.Configuration.Repositories;

namespace P3Model.Parser.Configuration;

public class P3 : P3.RepositoriesStep, P3.AnalyzersStep, P3.OutputFormatStep, P3.RootAnalyzerStep
{
    private readonly ProductBuilder _productBuilder = new();
    private readonly RepositoriesBuilder _repositoriesBuilder = new();
    private readonly AnalyzersBuilder _analyzersBuilder = new();
    private readonly OutputFormatBuilder _outputFormatBuilder = new();

    private P3() { }

    public static RepositoriesStep Product(Func<ProductBuilder, ProductBuilder> configure)
    {
        var p3 = new P3();
        configure(p3._productBuilder);
        return p3;
    }

    AnalyzersStep RepositoriesStep.Repositories(Func<RepositoriesBuilder, RepositoriesBuilder> configure)
    {
        configure(_repositoriesBuilder);
        return this;
    }

    OutputFormatStep AnalyzersStep.Analyzers(Func<AnalyzersBuilder, AnalyzersBuilder> configure)
    {
        configure(_analyzersBuilder);
        return this;
    }

    RootAnalyzerStep OutputFormatStep.OutputFormat(Func<OutputFormatBuilder, OutputFormatBuilder> configure)
    {
        configure(_outputFormatBuilder);
        return this;
    }

    RootAnalyzer RootAnalyzerStep.CreateRootAnalyzer()
    {
        var repositories = _repositoriesBuilder.Build();
        if (repositories.Count == 0)
            throw new InvalidOperationException("No repository to analyze");
        var productName = _productBuilder.Build();
        var allAnalyzers = _analyzersBuilder.Build();
        return new RootAnalyzer(productName,
            repositories,
            allAnalyzers.FileAnalyzers,
            allAnalyzers.SymbolAnalyzers,
            _outputFormatBuilder.Build());
    }
    
    public interface RepositoriesStep
    {
        AnalyzersStep Repositories(Func<RepositoriesBuilder, RepositoriesBuilder> configure);
    }
    
    public interface AnalyzersStep
    {
        OutputFormatStep Analyzers(Func<AnalyzersBuilder, AnalyzersBuilder> builder);
    }

    public interface OutputFormatStep
    {
        RootAnalyzerStep OutputFormat(Func<OutputFormatBuilder, OutputFormatBuilder> builder);
    }

    public interface RootAnalyzerStep
    {
        async Task Analyze() => await CreateRootAnalyzer().Analyze();
        RootAnalyzer CreateRootAnalyzer();
    }
}