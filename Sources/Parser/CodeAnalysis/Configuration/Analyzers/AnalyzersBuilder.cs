using System;
using System.Collections.Generic;
using System.Linq;
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
        var domainModuleFinder = new DomainModulesHierarchyResolvers(
            new NamespaceBasedDomainModulesHierarchyResolver(options.NamespaceOptions.Filter));
        var domainModuleAnalyzer = new DomainModuleAnalyzer(domainModuleFinder);
        var domainBuildingBlockAnalyzer = new DomainBuildingBlockAnalyzer(domainModuleFinder);
        var dddAggregateAnalyzer = new DddAggregateAnalyzer(domainModuleFinder);
        var dddDomainServiceAnalyzer = new DddDomainServiceAnalyzer(domainModuleFinder);
        var dddEntityAnalyzer = new DddEntityAnalyzer(domainModuleFinder);
        var dddFactoryAnalyzer = new DddFactoryAnalyzer(domainModuleFinder);
        var dddRepositoryAnalyzer = new DddRepositoryAnalyzer(domainModuleFinder);
        var dddValueObjectAnalyzer = new DddValueObjectAnalyzer(domainModuleFinder);
        var externalSystemIntegrationAnalyzer = new ExternalSystemIntegrationAnalyzer(domainModuleFinder);
        var useCaseAnalyzer = new UseCaseAnalyzer(domainModuleFinder);
        _symbolAnalyzers.Add(domainModuleAnalyzer);
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
        _fileAnalyzers.Add(domainModuleAnalyzer);
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