using JetBrains.Annotations;
using P3Model.Parser.CodeAnalysis.Domain;
using P3Model.Parser.CodeAnalysis.Domain.Ddd;

namespace P3Model.Parser.CodeAnalysis.Configuration.Analyzers;

public class AnalyzersBuilder
{
    private readonly List<FileAnalyzer> _fileAnalyzers = new();
    private readonly List<SymbolAnalyzer> _symbolAnalyzers = new();
    private readonly List<OperationAnalyzer> _operationAnalyzers = new();

    [PublicAPI]
    public AnalyzersBuilder UseDefaults(
        Func<DefaultAnalyzersOptionsBuilder, DefaultAnalyzersOptionsBuilder>? configure = null)
    {
        var builder = new DefaultAnalyzersOptionsBuilder();
        configure?.Invoke(builder);
        _symbolAnalyzers.AddRange(CreateAnalyzersWithParameterlessConstructor<SymbolAnalyzer>());
        var options = builder.Build();
        var modelBoundaryAnalyzer = new ModelBoundaryAnalyzer();
        var namespaceBasedDomainModuleAnalyzer = new NamespaceBasedDomainModuleAnalyzer(
            options.NamespaceOptions.NamespacePartsToSkip);
        var domainModuleAnalyzers = new DomainModuleAnalyzers(namespaceBasedDomainModuleAnalyzer);
        var domainBuildingBlockAnalyzer = new DomainBuildingBlockAnalyzer(modelBoundaryAnalyzer, domainModuleAnalyzers);
        var dddAggregateAnalyzer = new DddAggregateAnalyzer(modelBoundaryAnalyzer, domainModuleAnalyzers);
        var dddDomainServiceAnalyzer = new DddDomainServiceAnalyzer(modelBoundaryAnalyzer, domainModuleAnalyzers);
        var dddEntityAnalyzer = new DddEntityAnalyzer(modelBoundaryAnalyzer, domainModuleAnalyzers);
        var dddFactoryAnalyzer = new DddFactoryAnalyzer(modelBoundaryAnalyzer, domainModuleAnalyzers);
        var dddRepositoryAnalyzer = new DddRepositoryAnalyzer(modelBoundaryAnalyzer, domainModuleAnalyzers);
        var dddValueObjectAnalyzer = new DddValueObjectAnalyzer(modelBoundaryAnalyzer, domainModuleAnalyzers);
        var externalSystemIntegrationAnalyzer = new ExternalSystemIntegrationAnalyzer(modelBoundaryAnalyzer, 
            domainModuleAnalyzers);
        var useCaseAnalyzer = new UseCaseAnalyzer(modelBoundaryAnalyzer, domainModuleAnalyzers);
        _symbolAnalyzers.Add(modelBoundaryAnalyzer);
        _symbolAnalyzers.Add(namespaceBasedDomainModuleAnalyzer);
        _symbolAnalyzers.Add(domainBuildingBlockAnalyzer);
        _symbolAnalyzers.Add(dddAggregateAnalyzer);
        _symbolAnalyzers.Add(dddDomainServiceAnalyzer);
        _symbolAnalyzers.Add(dddEntityAnalyzer);
        _symbolAnalyzers.Add(dddFactoryAnalyzer);
        _symbolAnalyzers.Add(dddRepositoryAnalyzer);
        _symbolAnalyzers.Add(dddValueObjectAnalyzer);
        _symbolAnalyzers.Add(externalSystemIntegrationAnalyzer);
        _symbolAnalyzers.Add(useCaseAnalyzer);
        _fileAnalyzers.AddRange(CreateAnalyzersWithParameterlessConstructor<FileAnalyzer>());
        _operationAnalyzers.AddRange(CreateAnalyzersWithParameterlessConstructor<OperationAnalyzer>());
        return this;
    }

    private static IEnumerable<T> CreateAnalyzersWithParameterlessConstructor<T>() => typeof(AnalyzersBuilder).Assembly
        .GetTypes()
        .Where(t => typeof(T).IsAssignableFrom(t))
        .Where(t =>
        {
            var constructors = t.GetConstructors();
            return constructors.Length == 1 && constructors[0].GetParameters().Length == 0;
        })
        .Select(Activator.CreateInstance)
        .Cast<T>();

    [PublicAPI]
    public AnalyzersBuilder Including(params FileAnalyzer[] analyzers)
    {
        _fileAnalyzers.AddRange(analyzers);
        return this;
    }

    [PublicAPI]
    public AnalyzersBuilder ExcludingFileAnalyzer<TAnalyzer>()
        where TAnalyzer : class, FileAnalyzer
    {
        _fileAnalyzers.RemoveAll(analyzer => analyzer is TAnalyzer);
        return this;
    }

    [PublicAPI]
    public AnalyzersBuilder Including(params SymbolAnalyzer[] analyzers)
    {
        _symbolAnalyzers.AddRange(analyzers);
        return this;
    }

    [PublicAPI]
    public AnalyzersBuilder ExcludingSymbolAnalyzer<TAnalyzer>()
        where TAnalyzer : class, SymbolAnalyzer
    {
        _symbolAnalyzers.RemoveAll(analyzer => analyzer is TAnalyzer);
        return this;
    }

    public AllAnalyzers Build() => new(_fileAnalyzers.AsReadOnly(),
        _symbolAnalyzers.AsReadOnly(),
        _operationAnalyzers.AsReadOnly());
}