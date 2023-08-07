using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.CodeAnalysis;
using P3Model.Parser.CodeAnalysis.DomainPerspective;

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
        _symbolAnalyzers.AddRange(CreateAnalyzersWithParameterlessConstructor<SymbolAnalyzer>());
        var options = builder.Build();
        var domainModuleAnalyzer = new DomainModuleAnalyzer(
            options.NamespaceOptions.Predicate,
            options.NamespaceOptions.Filter);
        _symbolAnalyzers.Add(domainModuleAnalyzer);
        _fileAnalyzers.AddRange(CreateAnalyzersWithParameterlessConstructor<FileAnalyzer>());
        _fileAnalyzers.Add(domainModuleAnalyzer);
        return this;
    }
    
    private static IEnumerable<T> CreateAnalyzersWithParameterlessConstructor<T>() => typeof(T).Assembly
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

    public AllAnalyzers Build() => new(_fileAnalyzers.AsReadOnly(), _symbolAnalyzers.AsReadOnly());
}