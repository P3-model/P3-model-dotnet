using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.CodeAnalysis;
using P3Model.Parser.CodeAnalysis.DomainPerspective.StaticModel;
using P3Model.Parser.CodeAnalysis.DomainPerspective.StaticModel.Ddd;
using P3Model.Parser.CodeAnalysis.People;

namespace P3Model.Parser.Configuration.Analyzers;

public class AnalyzersBuilder
{
    private readonly List<FileAnalyzer> _fileAnalyzers = new();
    private readonly List<SymbolAnalyzer> _symbolAnalyzers = new();

    [PublicAPI]
    public AnalyzersBuilder UseDefaults(
        Func<DefaultAnalyzersOptionsBuilder, DefaultAnalyzersOptionsBuilder>? configure = null)
    {
        var builder = new DefaultAnalyzersOptionsBuilder();
        configure?.Invoke(builder);
        var options = builder.Build();
        _symbolAnalyzers.Add(new DomainModuleAnalyzer(
            options.NamespaceOptions.Predicate,
            options.NamespaceOptions.Filter));
        _symbolAnalyzers.Add(new ModelBoundaryAnalyzer());
        _symbolAnalyzers.Add(new DddValueObjectAnalyzer());
        _symbolAnalyzers.Add(new DddEntityAnalyzer());
        _symbolAnalyzers.Add(new DddAggregateAnalyzer());
        _symbolAnalyzers.Add(new DddDomainServiceAnalyzer());
        _symbolAnalyzers.Add(new DddFactoryAnalyzer());
        _symbolAnalyzers.Add(new DddRepositoryAnalyzer());
        _symbolAnalyzers.Add(new ActorAnalyzer());
        return this;
    }
    
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

    public AllAnalyzers Build() => new(_fileAnalyzers.AsReadOnly(), _symbolAnalyzers.AsReadOnly());
}