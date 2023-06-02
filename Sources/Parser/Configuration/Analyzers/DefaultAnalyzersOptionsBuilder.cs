using System;

namespace P3Model.Parser.Configuration.Analyzers;

public class DefaultAnalyzersOptionsBuilder
{
    private NamespaceOptions _namespaceOptions = new(_ => false, _ => string.Empty);

    public DefaultAnalyzersOptionsBuilder TreatNamespacesAsDomainModules(
        Func<NamespaceOptionsBuilder, NamespaceOptionsBuilder>? configure = null)
    {
        var namespaceOptionsBuilder = new NamespaceOptionsBuilder();
        configure?.Invoke(namespaceOptionsBuilder);
        _namespaceOptions = namespaceOptionsBuilder.Build();
        return this;
    }

    public DefaultAnalyzersOptions Build() => new(_namespaceOptions);
}