using System;
using Microsoft.CodeAnalysis;

namespace P3Model.Parser.Configuration.Analyzers;

public record NamespaceOptions(Func<INamespaceSymbol, string> Filter);